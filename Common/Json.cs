using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DnD4e.LibraryHelper.Common {
    public class Json : ILibrary {
        private SortedDictionary<string, Combatant> combatants = new SortedDictionary<string, Combatant>();
        private string characterFile;
        private string monsterFile;

        public Json () { }

        public IQueryable<Character.Character> Characters {
            get { return this.Query<Character.Character>().AsQueryable(); }
        }

        public IQueryable<Combatant> Combatants {
            get { return this.Query<Combatant>().AsQueryable(); }
        }

        public IQueryable<Monster.Monster> Monsters {
            get { return this.Query<Monster.Monster>().AsQueryable(); }
        }

        public bool Exists (Combatant combatant) {
            if (combatant == null) {
                return false;
            }
            else {
                return this.combatants.ContainsKey(combatant.Handle);
            }
        }

        public IEnumerable<T> Query<T> () where T : Combatant {
            return this.combatants.Values.OfType<T>();
        }

        public IEnumerable<T> QueryByName<T> (string name) where T : Combatant {
            return from c in this.Query<T>()
                   where c.Name.IndexOf(name, StringComparison.OrdinalIgnoreCase) != -1
                   select c;
        }

        public IEnumerable<T> QueryByProperty<T> (string property, object value) where T : Combatant {
            throw new NotImplementedException();
        }

        public void Open (string filename) {
            var path = Path.GetDirectoryName(filename);
            var file = Path.GetFileNameWithoutExtension(filename);
            this.characterFile = Path.Combine(path, file + ".cdb");
            this.monsterFile = Path.Combine(path, file + ".mdb");

            if (File.Exists(this.characterFile)) {
                using (var stream = File.OpenText(this.characterFile)) {
                    string json = stream.ReadToEnd();
                    var tmp = JsonConvert.DeserializeObject<List<Character.Character>>(json);
                    foreach (var character in tmp) {
                        this.Upsert(character);
                    }
                }
            }

            if (File.Exists(this.monsterFile)) {
                using (var stream = File.OpenText(this.monsterFile)) {
                    string json = stream.ReadToEnd();
                    var tmp = JsonConvert.DeserializeObject<List<Monster.Monster>>(json);
                    foreach (var monster in tmp) {
                        this.Upsert(monster);
                    }
                }
            }
        }

        public void Remove (Combatant combatant) {
            if (combatant == null) {
                throw new ArgumentNullException("combatant");
            }

            if (this.combatants.ContainsKey(combatant.Handle)) {
                this.combatants.Remove(combatant.Handle);
            }
        }

        public void Upsert (Combatant combatant) {
            if (combatant == null) {
                throw new ArgumentNullException("combatant");
            }

            this.combatants[combatant.Handle] = combatant;
        }

        public void Dispose () {
            var chars = Task.Factory.StartNew(() => {
                using (var stream = File.CreateText(this.characterFile)) {
                    var json = JsonConvert.SerializeObject(this.combatants.Values.OfType<Character.Character>());
                    stream.Write(json);
                }
            });

            var mons = Task.Factory.StartNew(() => {
                using (var stream = File.CreateText(this.monsterFile)) {
                    var json = JsonConvert.SerializeObject(this.combatants.Values.OfType<Monster.Monster>());
                    stream.Write(json);
                }
            });

            Task.WaitAll(chars, mons);
        }
    }
}
