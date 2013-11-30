using System;
using DnD4e.LibraryHelper.Common;
using Newtonsoft.Json;

namespace DnD4e.LibraryHelper.Encounter {
    public class CombatantWrapper {
        private string handle;  // only use for deserialization

        public CombatantWrapper () { }

        [JsonConstructor]
        public CombatantWrapper (string handle) {
            this.handle = handle;
        }

        [JsonIgnore]
        public Combatant Combatant { get; internal set; }

        public int Count { get; set; }

        public string Handle { 
            get {
                if (this.Combatant == null) {
                    return this.handle;
                }
                else {
                    return this.Combatant.Handle;
                }
            } 
        }

        [JsonIgnore]
        public string Name { get { return this.Combatant.Name; } }

        [JsonIgnore]
        public int Level { get { return this.Combatant.Level; } }

        [JsonIgnore]
        public CombatantType Type { get { return this.Combatant.CombatantType; } }
    }
}
