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
        private ObservableKeyedCollection<string, CombatantWrapper> combatants;

        public Encounter () {
            this.combatants = new ObservableKeyedCollection<string, CombatantWrapper>(c => c.Handle);
            //this.Combatants.CollectionChanged += this.Combatants_CollectionChanged;
        }

        [JsonConstructor]
        public Encounter (string campaign, string name, ObservableKeyedCollection<string, CombatantWrapper> combatants) {
            // mostly here as a helper for JSON.Net
            this.Campaign = campaign;
            this.Name = name;
            this.Combatants = combatants;
        }

        public string Campaign { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public ObservableKeyedCollection<string, CombatantWrapper> Combatants {
            get { return this.combatants; }
            internal set {
                //if (this.combatants != null) {
                //    this.combatants.CollectionChanged -= this.Combatants_CollectionChanged;
                //}
                this.combatants = value;
                //this.combatants.CollectionChanged += this.Combatants_CollectionChanged;
            }
        }

        [JsonIgnore]
        public IEnumerable<Character.Character> Characters {
            get {
                return this.Combatants.Select(c => c.Combatant).OfType<Character.Character>();
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
                return this.Combatants.Select(c => c.Combatant).OfType<Monster.Monster>();
            }
        }

        [JsonIgnore]
        public int MonsterCount {
            get {
                return this.combatants.Where(c => c.Type == CombatantType.Monster).Sum(c => c.Count);
            }
        }

        [JsonIgnore]
        public int AverageLevel {
            get {
                return (int)this.Monsters.Average(m => m.Level);
            }
        }

        [JsonIgnore]
        public int EncounterLevel {
            get {
                if (this.CharacterCount == 0) {
                    return this.AverageLevel;
                }
                else {
                    return this.CalculateEncounterLevel(this.CharacterCount);
                }
            }
        }

        [JsonIgnore]
        public int TotalXP {
            get {
                return (int)this.Monsters.Sum(m => m.Experience);
            }
        }

        private int CalculateEncounterLevel (int partySize) {
            var encounterXP = this.TotalXP;
            if (encounterXP == 0 || partySize == 0) {
                return 0;
            }

            // TODO: create actual calculation rather than conditional logic
            int perPC = encounterXP / partySize;
            if (perPC <= 112) { return 1; }
            else if (perPC <= 137) { return 2; }
            else if (perPC <= 162) { return 3; }
            else if (perPC <= 187) { return 4; }
            else if (perPC <= 225) { return 5; }

            else if (perPC <= 275) { return 6; }
            else if (perPC <= 325) { return 7; }
            else if (perPC <= 375) { return 8; }
            else if (perPC <= 425) { return 9; }
            else if (perPC <= 475) { return 10; }

            else if (perPC <= 650) { return 11; }
            else if (perPC <= 750) { return 12; }
            else if (perPC <= 900) { return 13; }
            else if (perPC <= 1100) { return 14; }
            else if (perPC <= 1300) { return 15; }

            else if (perPC <= 1500) { return 16; }
            else if (perPC <= 1800) { return 17; }
            else if (perPC <= 2200) { return 18; }
            else if (perPC <= 2600) { return 19; }
            else if (perPC <= 3000) { return 20; }

            else if (perPC <= 3675) { return 21; }
            else if (perPC <= 4625) { return 22; }
            else if (perPC <= 5575) { return 23; }
            else if (perPC <= 6525) { return 24; }
            else if (perPC <= 8000) { return 25; }

            else if (perPC <= 10000) { return 26; }
            else if (perPC <= 12000) { return 27; }
            else if (perPC <= 14000) { return 28; }
            else if (perPC <= 17000) { return 29; }
            else if (perPC <= 21000) { return 30; }

            else if (perPC <= 25000) { return 31; }
            else if (perPC <= 29000) { return 32; }
            else if (perPC <= 35000) { return 33; }
            else if (perPC <= 43000) { return 34; }
            else if (perPC <= 51000) { return 35; }

            else if (perPC <= 59000) { return 36; }
            else if (perPC <= 71000) { return 37; }
            else if (perPC <= 87000) { return 38; }
            else if (perPC <= 103000) { return 39; }
            else { return 40; }
        }

        public override string ToString () {
            return this.Name;
        }
    }
}
