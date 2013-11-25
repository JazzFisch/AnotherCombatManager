using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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

        private const string CharactersKey = "Characters.json";
        private const string EncountersKey = "Encounters.json";
        private const string MonstersKey = "Monsters.json";

        private static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings() {
            Converters = new List<JsonConverter>() { new StringEnumConverter() }
        };
        private ObservableCombatantDictionary<Character.Character> characters = new ObservableCombatantDictionary<Character.Character>();
        private ObservableCombatantDictionary<Monster.Monster> monsters = new ObservableCombatantDictionary<Monster.Monster>();
        private bool dirty = false;
        private Rules d20Rules;
        private string libraryPath;

        #endregion

        #region Constructors

        private Library () { }

        public static async Task<Library> OpenLibraryAsync (string name = "Library.zip") {
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

        public ObservableCombatantDictionary<Character.Character> Characters { get { return this.characters; } }

        public ObservableCombatantDictionary<Monster.Monster> Monsters { get { return this.monsters; } }

        #endregion

        #region Public Methods

        public async Task FlushAsync () {
            try {
                if (!this.dirty) {
                    return;
                }

                var path = Path.GetTempPath();
                var tmp = Path.GetRandomFileName();
                var random = Path.Combine(path, tmp);
                var backup = String.Concat(this.libraryPath, ".bak");

                var filename = Path.GetFileName(this.libraryPath);
                using (var file = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, 0x1000, useAsync: true)) {
                    using (var archive = new ZipArchive(file, ZipArchiveMode.Update)) {
                        await this.WriteCombatantsAsync(archive, CharactersKey, this.characters).ConfigureAwait(false);
                        await this.WriteCombatantsAsync(archive, MonstersKey, this.monsters).ConfigureAwait(false);
                    }
                }
                this.dirty = false;
            }
            catch (System.Exception ex) {
                Trace.TraceError(ex.ToString());
                System.Diagnostics.Debugger.Break();
                throw;
            }
        }

        public async Task<Character.Character> ImportCharacterFromFileAsync (string filename) {
            var character = await Character.Character.LoadFromFileAsync(filename, this.d20Rules).ConfigureAwait(false);
            this.Characters[character.Handle] = character;
            return character;
        }

        public async Task<IEnumerable<Character.Character>> ImportCharactersFromFileAsync (IEnumerable<string> filenames) {
            var names = filenames.ToArray();
            var tasks = new Task<Character.Character>[filenames.Count()];
            this.Characters.FireEvents = false;
            for (int i = 0; i < tasks.Length; ++i) {
                tasks[i] = this.ImportCharacterFromFileAsync(names[i]);
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);
            this.Characters.FireEvents = true;
            this.Characters.RaiseCollectionChanged(System.Collections.Specialized.NotifyCollectionChangedAction.Reset);
            return tasks.Select(t => t.Result);
        }

        public async Task<Monster.Monster> ImportMonsterFromFileAsync (string filename) {
            var monster = await Monster.Monster.LoadFromFileAsync(filename).ConfigureAwait(false);
            this.Monsters[monster.Handle] = monster;
            return monster;
        }

        public async Task<IEnumerable<Monster.Monster>> ImportMonstersFromFileAsync (IEnumerable<string> filenames) {
            var names = filenames.ToArray();
            var tasks = new Task<Monster.Monster>[filenames.Count()];
            this.Monsters.FireEvents = false;
            for (int i = 0; i < tasks.Length; ++i) {
                tasks[i] = this.ImportMonsterFromFileAsync(names[i]);
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);
            this.Monsters.FireEvents = true;
            this.Monsters.RaiseCollectionChanged(System.Collections.Specialized.NotifyCollectionChangedAction.Reset);
            return tasks.Select(t => t.Result);
        }

        public async Task LoadRulesAsync () {
            if (this.d20Rules == null) {
                this.d20Rules = await D20Rules.LoadFromAppDataAsync().ConfigureAwait(false);
            }
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

        private async Task<Dictionary<string, T>> ReadCombatantsAsync<T> (ZipArchive archive, string key) where T : Combatant {
            ZipArchiveEntry entry = archive.GetEntry(key);
            if (entry == null) {
                return new Dictionary<string, T>();
            }

            using (var stream = entry.Open()) {
                using (var reader = new StreamReader(stream)) {
                    var json = await reader.ReadToEndAsync();
                    var output = await JsonConvert.DeserializeObjectAsync<Dictionary<string, T>>(json, JsonSettings);
                    return output;
                }
            }
        }

        private async Task<bool> OpenAsync (string filename, bool useBackup = false) {
            this.libraryPath = filename;
            if (!File.Exists(filename)) {
                return true;
            }

            Dictionary<string, Character.Character> characters;
            Dictionary<string, Monster.Monster> monsters;

            try {
                using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read, 0x1000, useAsync: true)) {
                    using (var archive = new ZipArchive(file, ZipArchiveMode.Read)) {
                        characters = await this.ReadCombatantsAsync<Character.Character>(archive, CharactersKey);
                        monsters = await this.ReadCombatantsAsync<Monster.Monster>(archive, MonstersKey);
                    }
                }
            }
            catch (System.Exception ex) {
                Trace.TraceError(ex.ToString());
                return false;
            }

            this.characters.FireEvents = false;
            foreach (var character in characters) {
                this.characters[character.Key] = character.Value;
            }
            this.characters.FireEvents = true;

            this.monsters.FireEvents = false;
            foreach (var monster in monsters) {
                this.monsters[monster.Key] = monster.Value;
            }
            this.monsters.FireEvents = true;

            return true;
        }

        private async Task WriteCombatantsAsync<T> (ZipArchive archive, string key, IDictionary<string, T> combatants) where T : Combatant {
            if (combatants == null || combatants.Count == 0) {
                return;
            }

            string json = await JsonConvert.SerializeObjectAsync(combatants, Formatting.Indented, JsonSettings);
            ZipArchiveEntry entry = archive.GetEntry(key);
            if (entry == null) {
                entry = archive.CreateEntry(key, CompressionLevel.Fastest);
            }

            using (var stream = entry.Open()) {
                using (var writer = new StreamWriter(stream)) {
                    await writer.WriteAsync(json);
                }
            }
        }

        #endregion
    }
}
