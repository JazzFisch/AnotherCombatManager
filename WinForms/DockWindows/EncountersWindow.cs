using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Windows.Forms;
using AnotherCM.Library.Encounter;
using BrightIdeasSoftware;
using WeifenLuo.WinFormsUI.Docking;

namespace AnotherCM.WinForms.DockWindows {
    public partial class EncountersWindow : DockContent {
        public event EventHandler<EncountersSelectionChangedEventArgs> SelectionChanged;

        private Notify.Tracker tracker;
        private TypedObjectListView<Encounter> typedView;
        private ObservableCollection<Encounter> encounters;

        public EncountersWindow () {
            InitializeComponent();
            this.typedView = new TypedObjectListView<Encounter>(this.objectListView);

            // setup text search throttle
            var textChanged = Observable.FromEventPattern(this.searchTextBox, "TextChanged").Select(x => ((TextBox)x.Sender).Text);
            textChanged.Throttle(TimeSpan.FromMilliseconds(300))
                       .ObserveOn(SynchronizationContext.Current)
                       .Subscribe(text => {
                           this.Search(text);
                       });
        }

        public ObservableCollection<Encounter> Encounters {
            get {
                return this.encounters;
            }
            set {
                this.objectListView.EmptyListMsg = null;
                if (this.tracker != null) {
                    this.tracker.Dispose();
                }
                this.encounters = value;
                this.objectListView.SetObjects(this.encounters);
                this.tracker = new Notify.Tracker();
                this.tracker.Track(this.encounters);
                this.tracker.Changed += tracker_Changed;
            }
        }

        protected virtual void OnSelectionChanged (EncountersSelectionChangedEventArgs e) {
            if (this.SelectionChanged != null) {
                this.SelectionChanged(this, e);
            }
        }

        private void objectListView_SelectionChanged (object sender, EventArgs e) {
            var selected = this.typedView.SelectedObjects;
            if (selected.Any()) {
                this.OnSelectionChanged(new EncountersSelectionChangedEventArgs(selected));
            }
        }

        private void searchTextBox_Cleared (object sender, EventArgs e) {
            this.objectListView.AdditionalFilter = null;
        }

        void tracker_Changed (Notify.Tracker tracker) {
            this.objectListView.SetObjects(this.encounters);
        }

        private void Search (string text) {
            this.objectListView.AdditionalFilter = TextMatchFilter.Contains(this.objectListView, text);
        }
    }

    public class EncountersSelectionChangedEventArgs : EventArgs {
        public EncountersSelectionChangedEventArgs (IEnumerable<Encounter> encounters) {
            this.Encounters = encounters;
        }

        public IEnumerable<Encounter> Encounters { get; private set; }
    }
}
