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

        private static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings() {
            Converters = new List<JsonConverter>() { new StringEnumConverter() },
            NullValueHandling = NullValueHandling.Ignore
        };
        private ObservableKeyedCollection<string, Character.Character> characters = new ObservableKeyedCollection<string, Character.Character>(c => c.Handle);
        private ObservableCollection<Encounter.Encounter> encounters = new ObservableCollection<Encounter.Encounter>();
        private ObservableKeyedCollection<string, Monster.Monster> monsters = new ObservableKeyedCollection<string, Monster.Monster>(m => m.Handle);
        private Rules d20Rules;
        private string libraryPath;

        #endregion

        #region Constructors

        private Library () { }

        public static async Task<Library> OpenLibraryAsync (string name = DefaultFileName) {
            var path = Assembly.GetExecutingAssembly().Location;
            return await Library.OpenLibraryAsync(Path.GetDirectoryName(path), name).ConfigureAwait(false);
        }

        public static async Task<Library> OpenLibraryAsync (string path, string name) {
            string filename = Path.Combine(path, name);
            Library library = new Library();
            await library.OpenAsync(filename).ConfigureAwait(false);
            return library;
        }

        #endregion

        #region Properties

        public ObservableKeyedCollection<string, Character.Character> Characters { get { return this.characters; } }

        public ObservableCollection<Encounter.Encounter> Encounters { get { return this.encounters; } }

        public ObservableKeyedCollection<string, Monster.Monster> Monsters { get { return this.monsters; } }

        public string FileName { get { return this.libraryPath; } }

        #endregion

        #region Public Methods

        public async Task FlushAsync () {
            try {
                var path = Path.GetTempPath();
                var tmp = Path.GetRandomFileName();
                var random = Path.Combine(path, tmp);
                var backup = String.Concat(this.libraryPath, ".bak");
                using (var file = new FileStream(this.libraryPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, 0x1000, useAsync: true)) {
                    using (var archive = new ZipArchive(file, ZipArchiveMode.Update)) {
                        await this.WriteEntryAsync(archive, CharactersKey, this.characters).ConfigureAwait(false);
                        await this.WriteEntryAsync(archive, MonstersKey, this.monsters).ConfigureAwait(false);
                        await this.WriteEntryAsync(archive, EncountersKey, this.encounters).ConfigureAwait(false);
                    }
                }
            }
            catch (System.Exception ex) {
                Trace.TraceError(ex.ToString());
                System.Diagnostics.Debugger.Break();
                throw;
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

        public Task SaveAsAsync (string filename) {
            this.libraryPath = filename;
            return this.FlushAsync();
        }

        #endregion

        #region IDisposable

        public void Dispose () {
            // perform in a new thread so UI thread isn't blocking
            // the callback
            Task.Factory.StartNew(() => {
                var task = this.FlushAsync();
                task.Wait();
            }).Wait();
        }

        #endregion

        #region Private methods

        private async Task<bool> OpenAsync (string filename, bool useBackup = false) {
            this.libraryPath = filename;
            if (!File.Exists(filename)) {
                return true;
            }

            try {
                using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read, 0x1000, useAsync: true)) {
                    using (var archive = new ZipArchive(file, ZipArchiveMode.Read)) {
                        // ZipArchive methods are not thread safe, so serializing access
                        var characterConverter = new ObservableKeyedCollectionConverter<string, Character.Character>(c => c.Handle);
                        var encounterConverter = new ObservableKeyedCollectionConverter<string, Encounter.CombatantWrapper>(c => c.Handle);
                        var monsterConverter = new ObservableKeyedCollectionConverter<string, Monster.Monster>(m => m.Handle);
                        this.characters = await this.ReadEntryAsync<ObservableKeyedCollection<string, Character.Character>>(archive, CharactersKey, characterConverter);
                        this.monsters = await this.ReadEntryAsync<ObservableKeyedCollection<string, Monster.Monster>>(archive, MonstersKey, monsterConverter);
                        this.encounters = await this.ReadEntryAsync<ObservableCollection<Encounter.Encounter>>(archive, EncountersKey, encounterConverter);
                    }
                }
            }
            catch (System.Exception ex) {
                Trace.TraceError(ex.ToString());
                return false;
            }

            // fixup encounter combatants
            foreach (var encounter in this.encounters) {
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

        private async Task<T> ReadEntryAsync<T> (ZipArchive archive, string key, JsonConverter converter = null) where T : new() {
            // TODO: use stream deserialization to ensure fewer objects on the LOH
            ZipArchiveEntry entry = archive.GetEntry(key);
            if (entry == null) {
                return new T();
            }

            using (var stream = entry.Open()) {
                using (var reader = new StreamReader(stream)) {
                    string json = await reader.ReadToEndAsync();
                    if (converter != null) {
                        return await Task.Factory.StartNew<T>(() => {
                            return JsonConvert.DeserializeObject<T>(json, converter);
                        });
                    }
                    else {
                        return await JsonConvert.DeserializeObjectAsync<T>(json);
                    }
                }
            }
        }

        private async Task WriteEntryAsync<T> (ZipArchive archive, string key, T entry) where T : IList {
            if (entry == null || entry.Count == 0) {
                return;
            }

            string json = await JsonConvert.SerializeObjectAsync(entry, Formatting.Indented, JsonSettings);

            ZipArchiveEntry zipEntry = archive.GetEntry(key);
            if (zipEntry != null) {
                zipEntry.Delete();
            }
            zipEntry = archive.CreateEntry(key, CompressionLevel.Fastest);

            using (var stream = zipEntry.Open()) {
                using (var writer = new StreamWriter(stream)) {
                    await writer.WriteAsync(json);
                }
            }
        }

        #endregion
    }
}
