using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DnD4e.LibraryHelper.Import.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DnD4e.LibraryHelper.Common {
    public class Library {
        // TODO: add lazy commit on throttle this[handle] sets / removes
        // TODO: add restoring from backup if open fails

        #region Fields

        public const string DefaultFileName = "Library.zip";

        private const string CharactersKey = "Characters.json";
        private const string EncountersKey = "Encounters.json";
        private const string MonstersKey = "Monsters.json";
        private Rules d20Rules;

        #endregion

        #region Constructors

        private Library () { 
            this.Characters = new ObservableKeyedCollection<string, Character.Character>(c => c.Handle);
            this.Encounters = new ObservableCollection<Encounter.Encounter>();
            this.Monsters = new ObservableKeyedCollection<string, Monster.Monster>(m => m.Handle);
        }

        public static async Task<Library> OpenLibraryAsync (string name = DefaultFileName) {
            var path = Assembly.GetExecutingAssembly().Location;
            return await Library.OpenLibraryAsync(Path.GetDirectoryName(path), name).ConfigureAwait(false);
        }

        public static async Task<Library> OpenLibraryAsync (string path, string name) {
            string filename = Path.Combine(path, name);
            Library library = new Library();
            await Task.Factory.StartNew(() => library.Open(filename));
            return library;
        }

        #endregion

        #region Properties

        public ObservableKeyedCollection<string, Character.Character> Characters { get; private set; }

        public ObservableCollection<Encounter.Encounter> Encounters { get; private set; }

        public ObservableKeyedCollection<string, Monster.Monster> Monsters { get; private set; }

        public string FileName { get; private set; }

        #endregion

        #region Public Methods

        public void Flush () {
            var path = Path.GetTempPath();
            var tmp = Path.GetRandomFileName();
            var random = Path.Combine(path, tmp);
            var backup = String.Concat(this.FileName, ".bak");
            using (var file = new FileStream(this.FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, 0x1000)) {
                using (var archive = new ZipArchive(file, ZipArchiveMode.Update)) {
                    this.WriteEntry(archive, CharactersKey, this.Characters);
                    this.WriteEntry(archive, MonstersKey, this.Monsters);
                    this.WriteEntry(archive, EncountersKey, this.Encounters);
                }
            }
        }

        public async Task<Character.Character> ImportCharacterFromFileAsync (string filename) {
            var character = await Character.Character.LoadFromFileAsync(filename, this.d20Rules).ConfigureAwait(false);
            this.Characters.Set(character);
            return character;
        }

        public async Task<IEnumerable<Character.Character>> ImportCharactersFromFileAsync (IEnumerable<string> filenames) {
            var names = filenames.ToArray();
            var tasks = new Task<Character.Character>[filenames.Count()];
            for (int i = 0; i < tasks.Length; ++i) {
                tasks[i] = this.ImportCharacterFromFileAsync(names[i]);
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);
            return tasks.Select(t => t.Result);
        }

        public async Task<Monster.Monster> ImportMonsterFromFileAsync (string filename) {
            var monster = await Monster.Monster.LoadFromFileAsync(filename).ConfigureAwait(false);
            this.Monsters.Set(monster);
            return monster;
        }

        public async Task<IEnumerable<Monster.Monster>> ImportMonstersFromFileAsync (IEnumerable<string> filenames) {
            var names = filenames.ToArray();
            var tasks = new Task<Monster.Monster>[filenames.Count()];
            for (int i = 0; i < tasks.Length; ++i) {
                tasks[i] = this.ImportMonsterFromFileAsync(names[i]);
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);
            return tasks.Select(t => t.Result);
        }

        public async Task LoadRulesAsync () {
            if (this.d20Rules == null) {
                this.d20Rules = await D20Rules.LoadFromAppDataAsync().ConfigureAwait(false);
            }
        }

        public void SaveAs (string filename) {
            this.FileName = filename;
            this.Flush();
        }

        #endregion

        #region IDisposable

        public void Dispose () {
            this.Flush();
        }

        #endregion

        #region Private methods

        private JsonSerializer CreateSerializer (JsonConverter converter = null) {
            var serializer = new JsonSerializer() {
                NullValueHandling = NullValueHandling.Ignore
            };
            serializer.Converters.Add(new StringEnumConverter());
            if (converter != null) {
                serializer.Converters.Add(converter);
            }
            return serializer;
        }

        private bool Open (string filename, bool useBackup = false) {
            this.FileName = filename;
            if (!File.Exists(filename)) {
                return true;
            }

            using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read, 0x1000, useAsync: true)) {
                using (var archive = new ZipArchive(file, ZipArchiveMode.Read)) {
                    // ZipArchive methods are not thread safe, so serializing access
                    var characterConverter = new ObservableKeyedCollectionConverter<string, Character.Character>(c => c.Handle);
                    var monsterConverter = new ObservableKeyedCollectionConverter<string, Monster.Monster>(m => m.Handle);
                    var characters = this.ReadEntry<ObservableKeyedCollection<string, Character.Character>>(archive, CharactersKey, characterConverter);
                    var monsters = this.ReadEntry<ObservableKeyedCollection<string, Monster.Monster>>(archive, MonstersKey, monsterConverter);
                    var encounters = this.ReadEntry<ObservableCollection<Encounter.Encounter>>(archive, EncountersKey);

                    if (characters != null) {
                        this.Characters = characters;
                    }
                    if (monsters != null) {
                        this.Monsters = monsters;
                    }
                    if (encounters != null) {
                        this.Encounters = encounters;
                    }
                }
            }

            // fixup encounter combatants
            foreach (var encounter in this.Encounters) {
                foreach (var combatant in encounter.Combatants) {
                    Monster.Monster monster;
                    Character.Character character;
                    if (this.Characters.TryGetValue(combatant.Handle, out character)) {
                        combatant.Combatant = character;
                    }
                    else if (this.Monsters.TryGetValue(combatant.Handle, out monster)) {
                        combatant.Combatant = monster;
                    }
                }
            }

            return true;
        }

        private T ReadEntry<T> (ZipArchive archive, string key, JsonConverter converter = null) where T : new() {
            ZipArchiveEntry entry = archive.GetEntry(key);
            if (entry == null) {
                return default(T);
            }

            var serializer = this.CreateSerializer(converter);
            using (var stream = entry.Open()) {
                using (var reader = new StreamReader(stream)) {
                    using (var json = new JsonTextReader(reader)) {
                        return serializer.Deserialize<T>(json);
                    }
                }
            }
        }

        private void WriteEntry<T> (ZipArchive archive, string key, T entry) where T : IList {
            if (entry == null || entry.Count == 0) {
                return;
            }

            ZipArchiveEntry zipEntry = archive.GetEntry(key);
            if (zipEntry != null) {
                zipEntry.Delete();
            }
            zipEntry = archive.CreateEntry(key, CompressionLevel.Fastest);

            var serializer = this.CreateSerializer();
            using (var stream = zipEntry.Open()) {
                using (var writer = new StreamWriter(stream)) {
                    using (var json = new JsonTextWriter(writer)) {
                        serializer.Serialize(json, entry);
                    }
                }
            }
        }

        #endregion
    }
}
