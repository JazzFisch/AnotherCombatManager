using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using DnD4e.LibraryHelper.Encounter;
using WeifenLuo.WinFormsUI.Docking;

namespace DnD4e.CombatManager.Test.DockWindows {
    public partial class EncountersWindow : DockContent {
        public event EventHandler<EncountersSelectionChangedEventArgs> SelectionChanged;

        private ObservableCollection<Encounter> encounters;

        public EncountersWindow () {
            InitializeComponent();
        }


        public ObservableCollection<Encounter> Encounters {
            get {
                return this.encounters;
            }
            set {
                if (this.encounters != null) {
                    this.encounters.CollectionChanged -= encounters_CollectionChanged;
                }
                this.encounters = value;
                this.objectListView.SetObjects(this.encounters);
                this.encounters.CollectionChanged += encounters_CollectionChanged;
            }
        }

        private void encounters_CollectionChanged (object sender, NotifyCollectionChangedEventArgs e) {
            this.objectListView.SetObjects(this.encounters);
        }

        private void objectListView_SelectionChanged (object sender, EventArgs e) {
            var selected = this.objectListView.SelectedObjects.OfType<Encounter>();
            if (selected.Any()) {
                this.OnSelectionChanged(new EncountersSelectionChangedEventArgs(selected));
            }
        }

        protected virtual void OnSelectionChanged (EncountersSelectionChangedEventArgs e) {
            if (this.SelectionChanged != null) {
                this.SelectionChanged(this, e);
            }
        }
    }

    public class EncountersSelectionChangedEventArgs : EventArgs {
        public EncountersSelectionChangedEventArgs (IEnumerable<Encounter> encounters) {
            this.Encounters = encounters;
        }

        public IEnumerable<Encounter> Encounters { get; private set; }
    }
}
