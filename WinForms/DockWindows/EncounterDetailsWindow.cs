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
        private bool dirty = false;
        private Encounter encounter;

        public EncounterDetailsWindow () {
            InitializeComponent();
        }

        public Encounter Encounter {
            get { return this.encounter; }
            set {
                if (this.dirty && !this.IsHidden) {
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
                this.dirty = false;
                this.encounter = value;
                this.objectListView.SetObjects(this.encounter.Combatants);
                this.Text = this.encounter.Name;
            }
        }

        private void objectListView_ModelCanDrop (object sender, ModelDropEventArgs e) {
            if (!e.SourceModels.OfType<Combatant>().Any()) {
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

            this.dirty = true;
            this.Text = String.Concat("* ", this.Text);

            //var handles = this.encounter.Handles;
            //var combatants = this.encounter.Combatants;
            //foreach (var combatant in copy) {
            //    int count;
            //    if (!handles.TryGetValue(combatant.Handle, out count)) {
            //        combatants.Add(combatant.Handle, combatant);
            //    }
            //    else if (combatant is Character) {
            //        continue; // only one copy of a character allowed
            //    }
            //    else {
            //        this.encounter.IncrementCombatant(combatant.Handle);
            //    }
            //}
        }
    }
}
