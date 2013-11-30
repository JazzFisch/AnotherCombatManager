using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using DnD4e.LibraryHelper.Common;
using WeifenLuo.WinFormsUI.Docking;

namespace DnD4e.CombatManager.Test.DockWindows {
    public partial class CombatantListWindow<T> : DockContent where T : Combatant {
        public event EventHandler<CombatantsSelectionChangedEventArgs<T>> SelectionChanged;

        private ObservableKeyedCollection<string, T> combatants;

        public CombatantListWindow () {
            InitializeComponent();
        }

        public ObservableKeyedCollection<string, T> Combatants {
            get {
                return this.combatants;
            }
            set {
                if (this.combatants != null) {
                    this.combatants.CollectionChanged -= combatants_CollectionChanged;
                }
                this.combatants = value;
                this.objectListView.SetObjects(this.combatants);
                this.combatants.CollectionChanged += combatants_CollectionChanged;
            }
        }

        void combatants_CollectionChanged (object sender, NotifyCollectionChangedEventArgs e) {
            this.objectListView.SetObjects(this.combatants);
        }

        protected virtual void OnSelectionChanged (CombatantsSelectionChangedEventArgs<T> e) {
            if (this.SelectionChanged != null) {
                this.SelectionChanged(this, e);
            }
        }

        private void objectListView_SelectionChanged (object sender, EventArgs e) {
            var selected = this.objectListView.SelectedObjects.OfType<T>();
            if (selected.Any()) {
                this.OnSelectionChanged(new CombatantsSelectionChangedEventArgs<T>(selected));
            }
        }
    }

    public class CombatantsSelectionChangedEventArgs<T> : EventArgs where T : Combatant {
        public CombatantsSelectionChangedEventArgs (IEnumerable<T> combatants) {
            this.Combatants = combatants;
        }

        public IEnumerable<T> Combatants { get; private set; }
    }
}
