using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using DnD4e.LibraryHelper.ExtensionMethods;
using DnD4e.LibraryHelper.Import.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DnD4e.LibraryHelper.Common {
    public class Library {
        // TODO: add lazy commit on throttle this[handle] sets / removes
        // TODO: add restoring from backup if open fails
        // TODO: add hash to Content class to determine if writing Library is necessary

        #region Fields

        private const string CharactersKey = "Characters.json";
        private const string MonstersKey = "Monsters.json";
        private const string TrapsKey = "Traps.json";

        private static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings() {
            Converters = new List<JsonConverter>() { new StringEnumConverter() }
        };
        private ConcurrentDictionary<string, Combatant> combatants = new ConcurrentDictionary<string, Combatant>();
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

        public Combatant this[string handle] {
            get { return this.combatants[handle]; }
            set { this.Add(value); }
        }

        public IQueryable<Character.Character> Characters { get { return this.AsQueryable<Character.Character>(); } }

        public IQueryable<Combatant> Combatants { get { return this.AsQueryable<Combatant>(); } }

        public IQueryable<Monster.Monster> Monsters { get { return this.AsQueryable<Monster.Monster>(); } }

        #endregion

        #region Public Methods

        public void Add (Combatant combatant) {
            if (combatant == null) {
                throw new ArgumentNullException("combatant");
            }

            this.dirty = true;
            this.combatants[combatant.Handle] = combatant;
        }

        public async Task FlushAsync () {
            try {
                if (!this.dirty) {
                    return;
                }

                var path = Path.GetTempPath();
                var tmp = Path.GetRandomFileName();
                var random = Path.Combine(path, tmp);
                var backup = String.Concat(this.libraryPath, ".bak");

                var characters = this.combatants.Values.OfType<Character.Character>()
                                                       .ToDictionary(c => c.Handle);
                var monsters = this.combatants.Values.OfType<Monster.Monster>()
                                                     .ToDictionary(m => m.Handle);

                var filename = Path.GetFileName(this.libraryPath);
                using (var file = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, 0x1000, useAsync: true)) {
                    using (var archive = new ZipArchive(file, ZipArchiveMode.Update)) {
                        await this.WriteCombatantsAsync(archive, CharactersKey, characters).ConfigureAwait(false);
                        await this.WriteCombatantsAsync(archive, MonstersKey, monsters).ConfigureAwait(false);
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

        public bool Exists (Combatant combatant) {
            return this.combatants.ContainsKey(combatant.Handle);
        }

        public async Task<Character.Character> LoadCharacterFromFileAsync (string filename) {
            var character = await Character.Character.LoadFromFileAsync(filename, this.d20Rules).ConfigureAwait(false);
            this.Add(character);
            return character;
        }

        public async Task<IEnumerable<Character.Character>> LoadCharactersFromFileAsync (IEnumerable<string> filenames) {
            var names = filenames.ToArray();
            var tasks = new Task<Character.Character>[filenames.Count()];
            for (int i = 0; i < tasks.Length; ++i) {
                tasks[i] = this.LoadCharacterFromFileAsync(names[i]);
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);
            return tasks.Select(t => t.Result);
        }

        public async Task<Monster.Monster> LoadMonsterFromFileAsync (string filename) {
            var monster = await Monster.Monster.LoadFromFileAsync(filename).ConfigureAwait(false);
            this.Add(monster);
            return monster;
        }

        public async Task<IEnumerable<Monster.Monster>> LoadMonstersFromFileAsync (IEnumerable<string> filenames) {
            var names = filenames.ToArray();
            var tasks = new Task<Monster.Monster>[filenames.Count()];
            for (int i = 0; i < tasks.Length; ++i) {
                tasks[i] = this.LoadMonsterFromFileAsync(names[i]);
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);
            return tasks.Select(t => t.Result);
        }

        public async Task LoadRulesAsync () {
            if (this.d20Rules == null) {
                this.d20Rules = await D20Rules.LoadFromAppDataAsync().ConfigureAwait(false);
            }
        }

        public IEnumerable<T> QueryByName<T> (string name) where T : Combatant {
            return from c in this.AsQueryable<T>()
                   where c.Name.IndexOf(name, StringComparison.OrdinalIgnoreCase) != -1
                   select c;
        }

        public Combatant Remove (Combatant combatant) {
            if (combatant == null) {
                throw new ArgumentNullException("combatant");
            }

            Combatant output;
            this.combatants.TryRemove(combatant.Handle, out output);
            this.dirty = true;
            return output;
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

        private IQueryable<T> AsQueryable<T> () {
            return this.combatants.Values.OfType<T>().AsQueryable();
        }

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

            foreach (var character in characters) {
                this[character.Key] = character.Value;
            }

            foreach (var monster in monsters) {
                this[monster.Key] = monster.Value;
            }

            return true;
        }

        private async Task WriteCombatantsAsync<T> (ZipArchive archive, string key, Dictionary<string, T> combatants) where T : Combatant {
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
