using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace DnD4e.LibraryHelper.Common {
    public class Library {
        // TODO: add lazy commit on throttle this[handle] sets / removes
        // TODO: add restoring from backup if open fails
        private ConcurrentDictionary<string, Combatant> combatants = new ConcurrentDictionary<string, Combatant>();
        private string filename;

        private Library () { }

        public static Library OpenLibrary (string name = "Library.json") {
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
            set { this.combatants[handle] = value; }
        }

        public IQueryable<Character.Character> Characters { get { return this.AsQueryable<Character.Character>(); } }

        public IQueryable<Combatant> Combatants { get { return this.AsQueryable<Combatant>(); } }

        public IQueryable<Monster.Monster> Monsters { get { return this.AsQueryable<Monster.Monster>(); } }

        //public IQueryable<Trap.Trap> Traps { get { return this.AsQueryable<Trap.Trap>(); } }

        public void Close () {
            var path = Path.GetTempPath();
            var tmp = Path.GetRandomFileName();
            var random = Path.Combine(path, tmp);

            Content content = new Content() {
                Characters = this.combatants.Values.OfType<Character.Character>()
                                                   .ToDictionary(c => c.Handle),
                Monsters   = this.combatants.Values.OfType<Monster.Monster>()
                                                   .ToDictionary(m => m.Handle),
                //Traps      = this.combatants.Values.OfType<Trap.Trap>()
                //                                   .ToDictionary(t => t.Handle);
            };

            string json = JsonConvert.SerializeObject(content);
            var exists = File.Exists(this.filename);
            using (var stream = File.CreateText(exists ? random : this.filename)) {
                stream.Write(json);
            }
            if (exists) {
                File.Replace(random, this.filename, this.filename + ".bak");
            }
        }

        public bool Exists (Combatant combatant) {
            return this.combatants.ContainsKey(combatant.Handle);
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

        private IQueryable<T> AsQueryable<T> () {
            return this.combatants.Values.OfType<T>().AsQueryable();
        }

        private void Open (string filename) {
            this.filename = filename;
            if (!File.Exists(filename)) {
                return;
            }

            Content content;
            using (var stream = File.OpenText(this.filename)) {
                string json = stream.ReadToEnd();
                content = JsonConvert.DeserializeObject<Content>(json);                
            }

            foreach (var character in content.Characters) {
                this[character.Key] = character.Value;
            }

            foreach (var monster in content.Monsters) {
                this[monster.Key] = monster.Value;
            }

            //foreach (var trap in content.Traps) {
            //    this[trap.Key] = trap.Value;
            //}
        }

        private class Content {
            public Dictionary<string, Character.Character> Characters = new Dictionary<string,Character.Character>();
            public Dictionary<string, Monster.Monster> Monsters = new Dictionary<string, Monster.Monster>();
            //private Dictionary<string, Trap.Trap> Traps = new Dictionary<string, Trap.Trap>();
        }
    }
}
