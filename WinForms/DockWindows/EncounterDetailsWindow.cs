using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using AnotherCM.Library.Common;
using AnotherCM.Library.Encounter;
using BrightIdeasSoftware;
using WeifenLuo.WinFormsUI.Docking;

namespace AnotherCM.WinForms.DockWindows {
    // TODO: listen for flush / save events
    public partial class EncounterDetailsWindow : DockContent {
        public event EventHandler<CombatantsSelectionChangedEventArgs<Combatant>> SelectionChanged;

        private bool dirty = false;
        private Encounter encounter;
        private Notify.Tracker tracker;

        public EncounterDetailsWindow () {
            InitializeComponent();
            this.objectListView.EmptyListMsg = "No encounter selected.";

            this.addColumn.AspectGetter = (row) => {
                var wrapper = row as CombatantWrapper;
                if (wrapper.RenderType == RenderType.Character) {
                    return String.Empty;
                }
                else {
                    return "+";
                }
            };
            this.removeColumn.AspectGetter = (row) => { return "-"; };
        }

        public Encounter Encounter {
            get { return this.encounter; }
            set {
                if (value == null) {
                    throw new ArgumentNullException("value");
                }
                else if (Object.ReferenceEquals(this.encounter, value)) {
                    return;
                }
                else if (this.dirty && !this.IsHidden) {
                    var result = MessageBox.Show(
                        "You have unsaved changes.\nDo you wish to discard your changes?",
                        "Discard warning",
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Warning
                    );
                    if (result != DialogResult.Yes) {
                        return;
                    }
                }

                if (this.tracker != null) {
                    this.tracker.Dispose();
                }

                this.objectListView.EmptyListMsg = String.Empty;
                this.dirty = false;
                this.encounter = value;
                this.Text = this.Encounter.Name;
                this.objectListView.SetObjects(this.Encounter.Combatants);
                this.tracker = new Notify.Tracker();
                this.tracker.Track(this.encounter);
                this.tracker.Changed += tracker_Changed;
            }
        }

        protected virtual void OnSelectionChanged (CombatantsSelectionChangedEventArgs<Combatant> e) {
            var changed = this.SelectionChanged;
            if (changed != null) {
                changed(this, e);
            }
        }

        private void objectListView_HyperlinkClicked (object sender, HyperlinkClickedEventArgs e) {
            var row = e.Model as CombatantWrapper;
            switch (e.Column.Text) {
                case "+":
                    this.Encounter.AddCombatant(row.Combatant);
                    break;

                case "-":
                    this.Encounter.RemoveCombatant(row.Combatant);
                    break;
            }

            // finally, don't attempt to browse to the URL
            e.Handled = true;
        }

        private void objectListView_ModelCanDrop (object sender, ModelDropEventArgs e) {
            if ((this.Encounter == null) || !e.SourceModels.OfType<Combatant>().Any()) {
                e.Effect = DragDropEffects.None;
                return;
            }

            e.Effect = DragDropEffects.Copy;
        }

        private void objectListView_ModelDropped (object sender, ModelDropEventArgs e) {
            var copy = new List<Combatant>(e.SourceModels.OfType<Combatant>());
            if (copy.Count == 0) {
                return;
            }

            foreach (var combatant in copy) {
                this.Encounter.AddCombatant(combatant);
            }
        }

        private void objectListView_SelectionChanged (object sender, EventArgs e) {
            var selected = this.objectListView.SelectedObjects.OfType<CombatantWrapper>().Select(w => w.Combatant);
            if (selected.Any()) {
                this.OnSelectionChanged(new CombatantsSelectionChangedEventArgs<Combatant>(selected));
            }
        }

        void tracker_Changed (Notify.Tracker tracker) {
            if (!this.dirty) {
                this.dirty = true;
                this.Text = String.Concat("* ", this.Text);
            }
            this.objectListView.SetObjects(this.Encounter.Combatants);
        }
    }
}
