using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using DnD4e.CombatManager.Test.ExtensionMethods;
using DnD4e.LibraryHelper.Character;
using DnD4e.LibraryHelper.Common;
using DnD4e.LibraryHelper.Monster;

namespace DnD4e.CombatManager.Test {
    public partial class StatLibraryForm : Form {
        private Dictionary<string, Combatant> combatants = new Dictionary<string, Combatant>();
        private CombatantType combatantType = CombatantType.Invalid;
        private Combatant combatantToView;

        public StatLibraryForm () {
            InitializeComponent();
        }

        #region Event Handlers

        private void StatLibraryForm_Load (object sender, EventArgs e) {
        }

        private void toolStripStatListLoadATButon_Click (object sender, EventArgs e) {
            this.AddFilesToStatsList<Monster>("Monster Files|*.monster");
        }

        private void toolStripStatListLoadCBButton_Click (object sender, EventArgs e) {
            this.AddFilesToStatsList<Monster>("Character Files|*.dnd4e");
        }

        private void toolStripStatListDeleteButton_Click (object sender, EventArgs e) {
            if (statsListBox.SelectedIndices.Count == 0) {
                return;
            }

            var selected = statsListBox.SelectedItems.OfType<Combatant>().ToList();
            var prompt = String.Format(
                "Are you sure you want to delete the\nfollowing entries from the Library?\n{0}",
                String.Join("    \n", selected.Select(c => c.Handle))
            );

            var result = MessageBox.Show(prompt, "Delete request", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result != DialogResult.OK) {
                return;
            }

            // TODO: update XP totals

            // TODO: remove from "add to battle" pane

            foreach (var combatant in selected) {
                this.statsListBox.Items.Remove(combatant);
                this.combatants.Remove(combatant.Handle);
            }
        }

        private void statsListBox_KeyDown (object sender, KeyEventArgs e) {
            if (e.Control && (e.KeyCode == Keys.A)) {
                this.statsListBox.BeginUpdate();
                var old = this.statsListBox.SelectionMode;
                for (int i = 0; i < this.statsListBox.Items.Count; i++) {
                    this.statsListBox.SetSelected(i, true);
                }
                this.statsListBox.SelectionMode = old;
                this.statsListBox.EndUpdate();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void statsListBox_SelectedIndexChanged (object sender, EventArgs e) {
            Combatant combatant = this.statsListBox.SelectedItem as Combatant;
            if (combatant == null) {
                return;
            }

            // TODO: make a more generic web page flipping method
            if (combatant is Monster) {
                this.combatantToView = combatant;
                if (this.combatantType != CombatantType.Monster) {
                    this.combatantType = CombatantType.Monster;
                    this.statDetailsWebBrowser.DocumentText = Properties.Resources.monsterStatblock_html;
                    this.statDetailsWebBrowser.DocumentCompleted += this.statDetailsWebBrowser_DocumentCompleted;
                }
                else {
                    this.RenderCombatantDetails(this.combatantToView);
                }
            }
            //else if (comatant is Character) {
            //}
        }

        private void statDetailsWebBrowser_DocumentCompleted (object sender, WebBrowserDocumentCompletedEventArgs e) {
            // ordering of the following is IMPORTANT
            // stop listening
            this.statDetailsWebBrowser.DocumentCompleted -= this.statDetailsWebBrowser_DocumentCompleted;

            // load our css in
            this.statDetailsWebBrowser.AddStyleSheet(Properties.Resources.normalize_css);
            //this.statDetailsWebBrowser.AddStyleSheet(Properties.Resources.statblock_css);

            // load our javascript in
            this.statDetailsWebBrowser.AddScriptElement(Properties.Resources.modernizr_2_6_2_js);
            this.statDetailsWebBrowser.AddScriptElement(Properties.Resources.underscore_js);
            this.statDetailsWebBrowser.AddScriptElement(Properties.Resources.jquery_1_10_2_js);
            this.statDetailsWebBrowser.AddScriptElement(Properties.Resources.knockout_3_0_0_debug_js);
            this.statDetailsWebBrowser.AddScriptElement(Properties.Resources.knockout_StringInterpolatingBindingProvider_js);
            
            // TODO: flip based upon type of combatant being viewed
            this.statDetailsWebBrowser.AddScriptElement(Properties.Resources.bindingHandlers_js);
            this.statDetailsWebBrowser.AddScriptElement(Properties.Resources.monsterStatblock_js);

            this.RenderCombatantDetails(this.combatantToView);
        }

        #endregion

        private void AddFilesToStatsList<T> (string filter) where T : Combatant {
            OpenFileDialog dialog = new OpenFileDialog() {
                Filter = filter + "|All files (*.*)|*.*",
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = true,
                ValidateNames = true
            };
            dialog.Filter = filter + "|All files (*.*)|*.*";
            DialogResult result = dialog.ShowDialog();

            if ((result != DialogResult.OK) || (dialog.FileNames.Length == 0)) {
                return;
            }

            foreach (var filename in dialog.FileNames) {
                Monster monster;
                Character character;
                Combatant combatant = null;
                if (Monster.TryCreateFromFile(filename, out monster)) {
                    combatant = monster;
                }
                else if (Character.TryCreateFromFile(filename, out character)) {
                    combatant = character;
                }
                else {
                    var msg = String.Format("Unable to import \"{0}\"", Path.GetFileName(filename));
                    MessageBox.Show(msg, "Import warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    continue;
                }

                if (this.combatants.ContainsKey(combatant.Handle)) {
                    var old = this.statsListBox.Items
                                                .OfType<Combatant>()
                                                .Where(m => m.Handle == monster.Handle);
                    this.statsListBox.Items.Remove(old.Single());
                }
                this.combatants[combatant.Handle] = combatant;
                this.statsListBox.Items.Add(combatant);
            }
        }

        private void RenderCombatantDetails (Combatant combatant) {
            try {
                string json = combatant.ToJson();

#if DEBUG
                // round trip checking
                Monster monster;
                Debug.Assert(Monster.TryCreateFromJson(json, out monster));
#endif

                this.statDetailsWebBrowser.Document.InvokeScript(
                    "renderStatBlock",
                    new object[] { json }
                );
            }
            catch (System.Exception ex) {
                Trace.WriteLine(ex);
            }
        }
    }
}
