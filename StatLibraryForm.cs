using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using DnD4e.CombatManager.Test.ExtensionMethods;
using DnD4e.LibraryHelper.Character;
using DnD4e.LibraryHelper.Common;
using DnD4e.LibraryHelper.Monster;
using Microsoft.Win32;

namespace DnD4e.CombatManager.Test {
    public partial class StatLibraryForm : Form {
        #region Fields

        private const string WebBrowserEmulationPath = @"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION";
        private static readonly CultureInfo UICulture = Thread.CurrentThread.CurrentUICulture;
        private CombatantType combatantType = CombatantType.Invalid;
        private Combatant combatantToView;
        private int lowLevel = 1;
        private int highLevel = 40;
        private Library library;

        #endregion

        #region Constructors

        public StatLibraryForm () {
            // fix IE rendering modes... *sigh*
            // perform prior to any other initialization
            this.BrowserRenderingRegistryKeys(addKeys: true);

            InitializeComponent();
            this.toolStripLowLevelTextBox.Text = lowLevel.ToString();
            this.toolStripHighLevelTextBox.Text = highLevel.ToString();

            var textChanged = Observable.FromEventPattern(this.toolStripNameTextBox, "TextChanged").Select(x => ((ToolStripTextBox)x.Sender).Text);
            textChanged.Throttle(TimeSpan.FromMilliseconds(300))
                       .ObserveOn(SynchronizationContext.Current)
                       .Subscribe(text => {
                           this.SetCombatants();
                       });
        }

        #endregion

        #region Event Handlers  

        private void StatLibraryForm_Load (object sender, EventArgs e) {
            this.library = Library.OpenLibrary();
            SetCombatants();
        }

        private void StatLibraryForm_FormClosing (object sender, FormClosingEventArgs e) {
            // remove the browser rendering keys
            this.BrowserRenderingRegistryKeys(addKeys: false);
            this.library.Dispose();
        }

        private void statDetailsWebBrowser_DocumentCompleted (object sender, WebBrowserDocumentCompletedEventArgs e) {
            // ordering of the following is IMPORTANT
            // stop listening
            this.statDetailsWebBrowser.DocumentCompleted -= this.statDetailsWebBrowser_DocumentCompleted;

            // catch futher errors
            this.statDetailsWebBrowser.Document.Window.Error += (errorSender, errorArgs) => {
                errorArgs.Handled = true;
                Trace.WriteLine(errorArgs.Description);
                System.Diagnostics.Debugger.Break();
            };

            // load our css in
            this.statDetailsWebBrowser.AddStyleSheet(Properties.Resources.statblock_css);

            // load our javascript in
            this.statDetailsWebBrowser.AddScriptElement(Properties.Resources.modernizr_2_6_2_js);
            this.statDetailsWebBrowser.AddScriptElement(Properties.Resources.underscore_js);
            this.statDetailsWebBrowser.AddScriptElement(Properties.Resources.knockout_3_0_0_debug_js);
            this.statDetailsWebBrowser.AddScriptElement(Properties.Resources.knockout_StringInterpolatingBindingProvider_js);
            this.statDetailsWebBrowser.AddScriptElement(Properties.Resources.ko_ninja_js);
            this.statDetailsWebBrowser.AddScriptElement(Properties.Resources.statblockHelpers_js);

            // TODO: flip based upon type of combatant being viewed
            this.statDetailsWebBrowser.AddScriptElement(Properties.Resources.bindingHandlers_js);
            this.statDetailsWebBrowser.AddScriptElement(Properties.Resources.monsterStatblock_js);

            this.RenderCombatantDetails(this.combatantToView);
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

        private void toolStripRoleComboBox_SelectedIndexChanged (object sender, EventArgs e) {
            this.SetCombatants();
        }

        private void toolStripItemClearButton_Click (object sender, EventArgs e) {
            this.toolStripNameTextBox.Clear();
            this.toolStripRoleComboBox.SelectedIndex = 0;
            this.lowLevel = 1;
            this.highLevel = 40;
            this.toolStripLowLevelTextBox.Text = this.lowLevel.ToString();
            this.toolStripHighLevelTextBox.Text = this.highLevel.ToString();
            this.SetCombatants();
        }

        private void toolStripStatListLoadATButon_Click (object sender, EventArgs e) {
            int? index = this.AddFilesToStatsList<Monster>("Monster Files|*.monster");
            if (index.HasValue) {
                this.statsListBox.ClearSelected();
                this.statsListBox.SelectedIndex = index.Value;
            }
        }

        private void toolStripStatListLoadCBButton_Click (object sender, EventArgs e) {
            int? index = this.AddFilesToStatsList<Character>("Character Files|*.dnd4e");
            if (index.HasValue) {
                this.statsListBox.ClearSelected();
                this.statsListBox.SelectedIndex = index.Value;
            }
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
                this.library.Remove(combatant);
            }
            SetCombatants();
        }

        private void toolStripHighLevelTextBox_Validating (object sender, CancelEventArgs e) {
            int num;
            if (!int.TryParse(this.toolStripHighLevelTextBox.Text, out num)) {
                this.toolStripHighLevelTextBox.Text = this.highLevel.ToString();
                e.Cancel = true;
            }
            this.highLevel = num;
            this.SetCombatants();
        }

        private void toolStripLowLevelTextBox_Validating (object sender, CancelEventArgs e) {
            int num;
            if (!int.TryParse(this.toolStripLowLevelTextBox.Text, out num)) {
                this.toolStripLowLevelTextBox.Text = this.lowLevel.ToString();
                e.Cancel = true;
            }
            this.lowLevel = num;
            this.SetCombatants();
        }

        #endregion

        #region Private Methods

        private int? AddFilesToStatsList<T> (string filter) where T : Combatant {
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
                return null;
            }

            int index = -1;
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

                if (this.library.Exists(monster)) {
                    var old = this.statsListBox.Items
                                               .OfType<Combatant>()
                                               .Where(m => m.Handle == monster.Handle);
                    this.statsListBox.Items.Remove(old.Single());
                }
                this.library[combatant.Handle] = combatant;
                index = this.statsListBox.Items.Add(combatant);
            }

            if (dialog.FileNames.Length == 1) {
                return index;
            }
            else {
                return null;
            }
        }

        private void BrowserRenderingRegistryKeys (bool addKeys) {
            try {
                var exeName = Path.GetFileName(Assembly.GetEntryAssembly().Location);
                var key = Registry.CurrentUser.OpenSubKey(WebBrowserEmulationPath, true);

                if (addKeys) {
                    key.SetValue(exeName, 8000, RegistryValueKind.DWord);
                }
                else if (!addKeys && key.GetValue(exeName) != null) {
                    key.DeleteValue(exeName);
                }

#if DEBUG
                // add the vshost
                var ext = Path.GetExtension(exeName);
                exeName = Path.GetFileNameWithoutExtension(exeName);
                exeName += ".vshost" + ext;
                if (addKeys) {
                    key.SetValue(exeName, 8000, RegistryValueKind.DWord);
                }
                else if (!addKeys && key.GetValue(exeName) != null) {
                    key.DeleteValue(exeName);
                }
#endif
            }
            catch (System.Exception ex) {
                Trace.WriteLine(ex);
                System.Diagnostics.Debugger.Break();
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
                System.Diagnostics.Debugger.Break();
            }
        }

        private void SetCombatants () {
            var role = this.toolStripRoleComboBox.SelectedItem as string;
            var name = this.toolStripNameTextBox.Text;

            IEnumerable<Combatant> combatants;
            if (!String.IsNullOrWhiteSpace(name)) {
                combatants = this.library.QueryByName<Combatant>(name);
            }
            else {
                combatants = this.library.Combatants;
            }

            var query = combatants.Where(c => c.Level >= this.lowLevel)
                                  .Where(c => c.Level <= this.highLevel);
            if (!String.IsNullOrWhiteSpace(role)) {
                query = query.Where(c => String.Equals(c.Role, role, StringComparison.OrdinalIgnoreCase));
            }

            var matches = query.Select(c => c);
            this.statsListBox.BeginUpdate();
            this.statsListBox.Items.Clear();
            this.statsListBox.Items.AddRange(matches.ToArray());
            this.statsListBox.EndUpdate();
        }

        #endregion
    }
}
