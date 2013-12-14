using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using AnotherCM.Library.Common;
using Newtonsoft.Json;

namespace AnotherCM.Library.Encounter {
    public class CombatantWrapper : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        private int count;
        private string handle;  // only use for deserialization
        private bool inReserve;

        public CombatantWrapper () { }

        [JsonConstructor]
        public CombatantWrapper (string handle) {
            this.handle = handle;
        }

        public CombatantWrapper (Combatant combatant) {
            this.Combatant = combatant;
            this.Count = 1;
        }

        [JsonIgnore]
        public Combatant Combatant { get; internal set; }

        public int Count {
            get { return this.count; }
            set {
                if (this.count == value) {
                    return;
                }
                this.count = value;
                this.OnPropertyChanged();
            }
        }

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

        public bool InReserve {
            get { return this.inReserve; }
            set {
                if (this.inReserve == value) {
                    return;
                }
                this.inReserve = value;
                this.OnPropertyChanged();
            }
        }

        [JsonIgnore]
        public string Name { get { return this.Combatant.Name; } }

        [JsonIgnore]
        public int Level { get { return this.Combatant.Level; } }

        [JsonIgnore]
        public RenderType RenderType { get { return this.Combatant.RenderType; } }

        protected virtual void OnPropertyChanged ([CallerMemberName]string propertyName = null) {
            var propertyChanged = this.PropertyChanged;
            if (propertyChanged != null) {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
