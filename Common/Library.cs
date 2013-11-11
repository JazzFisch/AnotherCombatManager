using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using DnD4e.LibraryHelper.ExtensionMethods;
using DnD4e.LibraryHelper.Import.Common;
using Newtonsoft.Json;

namespace DnD4e.LibraryHelper.Common {
    public class Library {
        // TODO: add lazy commit on throttle this[handle] sets / removes
        // TODO: add restoring from backup if open fails
        // TODO: add hash to Content class to determine if writing Library is necessary
        private ConcurrentDictionary<string, Combatant> combatants = new ConcurrentDictionary<string, Combatant>();
        private D20Rules rules;
        private string filename;

        private Library () { }

        public static Library OpenLibrary (string name = "Library.json.zip") {
            var path = Assembly.GetExecutingAssembly().Location;
            return Library.OpenLibrary(Path.GetDirectoryName(path), name);
        }

        public static Library OpenLibrary (string path, string name) {
            string filename = Path.Combine(path, name);
            Library library = new Library();
            library.Open(filename);
            return library;
        }

        public Combatant this[string handle] {
            get { return this.combatants[handle]; }
            set { this.Add(value); }
        }

        public IQueryable<Character.Character> Characters { get { return this.AsQueryable<Character.Character>(); } }

        public IQueryable<Combatant> Combatants { get { return this.AsQueryable<Combatant>(); } }

        public IQueryable<Monster.Monster> Monsters { get { return this.AsQueryable<Monster.Monster>(); } }

        public IQueryable<Trap.Trap> Traps { get { return this.AsQueryable<Trap.Trap>(); } }

        public void Add (Combatant combatant) {
            if (combatant == null) {
                throw new ArgumentNullException("combatant");
            }

            // special attempt to add missing character rules
            var character = combatant as Character.Character;
            if (character != null && rules != null) {
                // fixup powers
            }

            this.combatants[combatant.Handle] = combatant;
        }

        public void Add (string compendiumHtml, Uri url) {
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(compendiumHtml);
            foreach (var err in doc.ParseErrors) {
                Trace.WriteLine(err);
                System.Diagnostics.Debugger.Break();
            }

            // TODO: switch based upon URL
            var root = doc.GetElementbyId("detail");
            if (root == null) {
                return;
            }

            var title = root.SelectSingleNode("h1");
            var subHead = title.SelectSingleNode("span[@class='thSubHead']").FirstChild.InnerText.Trim();
            var levelParts = title.SelectSingleNode("span[@class='thLevel']").FirstChild.InnerText.Trim().Split();
            var xp = title.SelectSingleNode("span[@class='thLevel']/span[@class='thXP']").FirstChild.InnerText.Trim();

            var trap = new Trap.Trap() {
                Name = title.FirstChild.InnerText.Trim(),
                Type = subHead,
                Level = Int32.Parse(levelParts[1].Trim()),
                Role = levelParts[2].Trim(),
                Experience = Int32.Parse(xp.Substring(3)),
                CompendiumUrl = url.ToString()
            };
            trap.Handle = trap.ToHandle();

            this.Add(trap);
        }

        public void Close () {
            var path = Path.GetTempPath();
            var tmp = Path.GetRandomFileName();
            var random = Path.Combine(path, tmp);

            Content content = new Content() {
                Characters = this.combatants.Values.OfType<Character.Character>()
                                                   .ToDictionary(c => c.Handle),
                Monsters   = this.combatants.Values.OfType<Monster.Monster>()
                                                   .ToDictionary(m => m.Handle),
                Traps      = this.combatants.Values.OfType<Trap.Trap>()
                                                   .ToDictionary(t => t.Handle)
            };

            string json = JsonConvert.SerializeObject(content);
            var exists = File.Exists(this.filename);
            using (var file = File.Create(exists ? random : this.filename)) {
                using (var compress = new DeflateStream(file, CompressionLevel.Fastest)) {
                    using (var stream = new StreamWriter(compress)) {
                        stream.Write(json);
                    }
                }
            }

            if (exists) {
                File.Replace(random, this.filename, this.filename + ".bak");
            }
        }

        public bool Exists (Combatant combatant) {
            return this.combatants.ContainsKey(combatant.Handle);
        }

        public bool TryOpenRules () {
            return D20Rules.TryCreateFromAppData(out this.rules);
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
            return output;
        }

        #region IDisposable
        
        public void Dispose () {
            Close();
        }

        #endregion

        #region Private methods

        private IQueryable<T> AsQueryable<T> () {
            return this.combatants.Values.OfType<T>().AsQueryable();
        }

        private void Open (string filename) {
            this.filename = filename;
            if (!File.Exists(filename)) {
                return;
            }

            Content content;
            using (var file = File.OpenRead(filename)) {
                using (var deflate = new DeflateStream(file, CompressionMode.Decompress)) {
                    using (var stream = new StreamReader(deflate)) {
                        string json = stream.ReadToEnd();
                        content = JsonConvert.DeserializeObject<Content>(json);                
                    }
                }
            }

            foreach (var character in content.Characters) {
                this[character.Key] = character.Value;
            }

            foreach (var monster in content.Monsters) {
                this[monster.Key] = monster.Value;
            }

            foreach (var trap in content.Traps) {
                this[trap.Key] = trap.Value;
            }
        }

        #endregion

        private class Content {
            public Dictionary<string, Character.Character> Characters = new Dictionary<string,Character.Character>();
            public Dictionary<string, Monster.Monster> Monsters = new Dictionary<string, Monster.Monster>();
            public Dictionary<string, Trap.Trap> Traps = new Dictionary<string, Trap.Trap>();
        }
    }
}
