using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using DnD4e.LibraryHelper.Common;
using Newtonsoft.Json;

namespace DnD4e.LibraryHelper.Encounter {
    [DefaultProperty("Name")]
    public class Encounter {
        private bool handlesSet;
        private List<string> handles;
        private ReadOnlyCollection<string> handlesExt;
        private ObservableCollection<Combatant> combatants;

        public Encounter () {
            this.handles = new List<string>();
            this.handlesExt = new ReadOnlyCollection<string>(this.handles);
            this.combatants = new ObservableCollection<Combatant>();
            this.combatants.CollectionChanged += this.Combatants_CollectionChanged;
        }

        private void Combatants_CollectionChanged (object sender, NotifyCollectionChangedEventArgs e) {
            // reset handles
            // TODO: make thread safe?
            this.handles.Clear();
            this.handles.AddRange(this.combatants.Select(c => c.Handle));
        }

        public string Campaign { get; set; }

        public string Name { get; set; }

        public ReadOnlyCollection<string> Handles {
            get { return this.handlesExt; }
            set {
                if (this.handlesSet) {
                    throw new InvalidOperationException("Handles can only be set once.");
                }
                this.handlesExt = value;
                this.handlesSet = true;
            }
        }

        [JsonIgnore]
        public ObservableCollection<Combatant> Combatants {
            get { return this.combatants; }
            internal set {
                if (this.combatants != null) {
                    this.combatants.CollectionChanged -= this.Combatants_CollectionChanged;
                }
                this.combatants = value;
                this.combatants.CollectionChanged += this.Combatants_CollectionChanged;
            }
        }

        [JsonIgnore]
        public IEnumerable<Character.Character> Characters {
            get {
                return this.Combatants.OfType<Character.Character>();
            }
        }

        [JsonIgnore]
        public int CharacterCount {
            get {
                return this.Characters.Count();
            }
        }

        [JsonIgnore]
        public IEnumerable<Monster.Monster> Monsters {
            get {
                return this.Combatants.OfType<Monster.Monster>();
            }
        }

        [JsonIgnore]
        public int MonsterCount {
            get {
                return this.Monsters.Count();
            }
        }

        [JsonIgnore]
        public int AverageLevel {
            get {
                return (int)this.Combatants.OfType<Monster.Monster>().Average(m => m.Level);
            }
        }

        [JsonIgnore]
        public int TotalXP {
            get {
                return (int)this.Combatants.OfType<Monster.Monster>().Sum(m => m.Experience);
            }
        }

        public override string ToString () {
            return this.Name;
        }
    }
}
