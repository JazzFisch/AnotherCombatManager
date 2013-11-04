using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DnD4e.LibraryHelper.Common {
    public class Library {
        public static void DeleteLibrary (string name) {
            var path = Assembly.GetExecutingAssembly().Location;
            path = Path.Combine(Path.GetDirectoryName(path), name);
            File.Delete(path);
        }

        public static Library OpenLibrary (string name) {
            var path = Assembly.GetExecutingAssembly().Location;
            return Library.OpenLibrary(Path.GetDirectoryName(path), name);
        }

        public static Library OpenLibrary (string path, string name) {
            Library library = new Library();
            return library;
        }

        public void Put (Combatant combatant) {
        }

        public void Remove (Combatant combatant) {
        }

        public void Set (Combatant combatant) {
        }
    }
}
