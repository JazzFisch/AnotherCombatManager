using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DnD4e.LibraryHelper.Common {
    public class Library : ILibrary {
        private ILibrary library;

        private Library () { }

        public static Library OpenLibrary (string name) {
            var path = Assembly.GetExecutingAssembly().Location;
            return Library.OpenLibrary(Path.GetDirectoryName(path), name);
        }

        public static Library OpenLibrary (string path, string name) {
            string filename = Path.Combine(path, name);
            Library library = new Library();
            library.Open(filename);
            return library;
        }

        public IQueryable<Character.Character> Characters { get { return this.library.Characters; } }

        public IQueryable<Combatant> Combatants { get { return this.library.Combatants; } }

        public IQueryable<Monster.Monster> Monsters { get { return this.library.Monsters; } }

        public bool Exists (Combatant combatant) {
            return this.library.Exists(combatant);
        }

        public IEnumerable<T> Query<T> () where T: Combatant {
            return this.library.Query<T>();
        }

        public IEnumerable<T> QueryByName<T> (string name) where T : Combatant {
            return this.library.QueryByName<T>(name);
        }

        public IEnumerable<T> QueryByProperty<T> (string property, object value) where T : Combatant {
            return this.library.QueryByProperty<T>(property, value);
        }

        public void Open (string filename) {
            //this.library = new NDatabase();
            //this.library = new Sterling();
            this.library = new Json();
            this.library.Open(filename);
        }

        public void Remove (Combatant combatant) {
            this.library.Remove(combatant);
        }

        public void Upsert (Combatant combatant) {
            this.library.Upsert(combatant);
        }

        #region IDisposable
        
        public void Dispose () {
            this.library.Dispose();
            this.library = null;
        }

        #endregion
    }
}
