using System;
using System.Collections.Generic;
using System.Linq;
using DnD4e.LibraryHelper.Common;
using WeifenLuo.WinFormsUI.Docking;

namespace DnD4e.CombatManager.Test.DockWindows {
    public partial class CombatantListWindow<T> : DockContent where T : Combatant {
        public event EventHandler<CombatantsSelectionChangedEventArgs<T>> SelectionChanged;

        public CombatantListWindow () {
            InitializeComponent();
        }

        public IEnumerable<T> Combatants {
            get {
                var objects = this.objectListView.Objects;
                if (objects == null) {
                    return new T[0];
                }
                return objects.OfType<T>();
            }
            set {
                this.objectListView.SetObjects(value);
            }
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
