using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DnD4e.CombatManager.Test.ExtensionMethods;
using DnD4e.LibraryHelper.Character;
using DnD4e.LibraryHelper.Common;
using DnD4e.LibraryHelper.Monster;
using DnD4e.LibraryHelper.Trap;
using Microsoft.Win32;

namespace DnD4e.CombatManager.Test {
    public partial class StatLibraryForm : Form {
        #region Fields

        public static readonly Uri CompendiumBaseUrl = new Uri("https://www.wizards.com/dndinsider/compendium/display.aspx");
        private const string LoginPath = "/dndinsider/compendium/login.aspx";
        private const string WebBrowserEmulationPath = @"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION";
        private static readonly CultureInfo UICulture = Thread.CurrentThread.CurrentUICulture;
        private CombatantType combatantType = CombatantType.Invalid;
        private string compendiumCookies;
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

        private async void StatLibraryForm_Load (object sender, EventArgs e) {
            TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
            this.statsListBox.Items.Add("Please wait, loading the goodness...");

            // the general idea is to get off the UI thread as soon as possible
            // while we load various parts of the library in the background
            this.library = await Library.OpenLibraryAsync();
            SetCombatants();

            await this.library.LoadRulesAsync();
            this.toolStripStatListLoadCBButton.Enabled = true;
        }

        private void StatLibraryForm_FormClosing (object sender, FormClosingEventArgs e) {
            // remove the browser rendering keys
            this.BrowserRenderingRegistryKeys(addKeys: false);
            this.library.Dispose();
        }

        private void statDetailsWebBrowser_CompendiumCompleted (object sender, WebBrowserDocumentCompletedEventArgs e) {
            this.statDetailsWebBrowser.AllowNavigation = false;
            this.statDetailsWebBrowser.DocumentCompleted -= this.statDetailsWebBrowser_CompendiumCompleted;
            var url = this.statDetailsWebBrowser.Url;
            if (!url.LocalPath.Equals(LoginPath, StringComparison.OrdinalIgnoreCase)) {
                this.compendiumCookies = this.statDetailsWebBrowser.Document.Cookie;
                this.library.Add(this.statDetailsWebBrowser.DocumentText, url);
                this.SetCombatants();
            }
            else {
                if (this.statDetailsWebBrowser.EncryptionLevel == WebBrowserEncryptionLevel.Insecure) {
                    // why does wizards redirect to an insecure url?!
                    // re-redirect to a secure login page... if you can believe this
                    // this makes our code more secure than theirs... *sigh*
                    this.statDetailsWebBrowser.Navigate(String.Format(
                        "https://{0}{1}{2}",
                        url.DnsSafeHost,
                        url.LocalPath,
                        url.Query
                    ));
                }
                this.statDetailsWebBrowser.AllowNavigation = true;
                this.statDetailsWebBrowser.DocumentCompleted += this.statDetailsWebBrowser_CompendiumCompleted;
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

            this.RenderCombatant(combatant);
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

        private async void toolStripStatListLoadATButon_Click (object sender, EventArgs e) {
            int? index = await this.AddFilesToStatsListAsync(CombatantType.Monster);
            if (index.HasValue) {
                this.statsListBox.ClearSelected();
                this.statsListBox.SelectedIndex = index.Value;
            }
        }

        private async void toolStripStatListLoadCBButton_Click (object sender, EventArgs e) {
            int? index = await this.AddFilesToStatsListAsync(CombatantType.Character);
            if (index.HasValue) {
                this.statsListBox.ClearSelected();
                this.statsListBox.SelectedIndex = index.Value;
            }
        }

        private void toolStripStatListLoadCompendiumButton_Click (object sender, EventArgs e) {
            //// get the URL off the clipboard
            //if (!Clipboard.ContainsText()) {
            //    return;
            //}

            //var text = Clipboard.GetText();
            //Uri url;
            //if (!Uri.TryCreate(text, UriKind.Absolute, out url)) {
            //    return;
            //}

            //if (!CompendiumBaseUrl.Host.Equals(url.Host, StringComparison.OrdinalIgnoreCase)) {
            //    return;
            //}
            //else if (!CompendiumBaseUrl.LocalPath.Equals(url.LocalPath, StringComparison.OrdinalIgnoreCase)) {
            //    return;
            //}

            //this.combatantToView = null;
            //this.statDetailsWebBrowser.DocumentCompleted += this.statDetailsWebBrowser_CompendiumCompleted;
            //this.statDetailsWebBrowser.AllowNavigation = true;

            //if (String.IsNullOrWhiteSpace(this.compendiumCookies)) {
            //    this.statDetailsWebBrowser.Navigate(url);
            //}
            //else {
            //    var cookies = String.Format("Cookie {0}", this.compendiumCookies);
            //    this.statDetailsWebBrowser.Navigate(url, String.Empty, null, cookies);
            //}
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

        private void RenderCombatant (Combatant combatant) {
            bool correctPage = this.combatantType == combatant.CombatantType;
            this.combatantType = combatant.CombatantType;
            Task<string> serializeTask = combatant.ToJsonAsync();
            if (correctPage) {
                this.RenderCombatantDetails(serializeTask);
                return;
            }

            string html = null;
            WebBrowserDocumentCompletedEventHandler completedHandler = null;
            if (combatant is Character) {
                html = Properties.Resources.characterStatblock_html;
                completedHandler = (a, b) => {
                    this.StopListeningAndAddCommonHtmlElements(completedHandler);
                    this.statDetailsWebBrowser.AddScriptElement(Properties.Resources.characterStatblock_js);
                    this.RenderCombatantDetails(serializeTask);
                };
            }
            else if (combatant is Monster) {
                html = Properties.Resources.monsterStatblock_html;
                completedHandler = (a, b) => {
                    this.StopListeningAndAddCommonHtmlElements(completedHandler);
                    this.statDetailsWebBrowser.AddScriptElement(Properties.Resources.monsterStatblock_js);
                    this.RenderCombatantDetails(serializeTask);
                };
            }
            else if (combatant is Trap) {
                html = Properties.Resources.trapStatblock_html;
                completedHandler = (a, b) => {
                    this.StopListeningAndAddCommonHtmlElements(completedHandler);
                    this.statDetailsWebBrowser.AddScriptElement(Properties.Resources.trapStatblock_js);
                    this.RenderCombatantDetails(serializeTask);
                };
            }
            else {
                return;
            }

            this.statDetailsWebBrowser.AllowNavigation = true;
            this.statDetailsWebBrowser.DocumentText = html;
            this.statDetailsWebBrowser.DocumentCompleted += completedHandler;
        }

        private void StopListeningAndAddCommonHtmlElements (WebBrowserDocumentCompletedEventHandler completedHandler) {
            // ordering of the following is IMPORTANT
            // stop listening
            this.statDetailsWebBrowser.AllowNavigation = false;
            this.statDetailsWebBrowser.DocumentCompleted -= completedHandler;

            // load our css in
            this.statDetailsWebBrowser.AddStyleSheet(Properties.Resources.statblock_css);

            // load our javascript in
            this.statDetailsWebBrowser.AddScriptElement(Properties.Resources.modernizr_2_6_2_js);
            this.statDetailsWebBrowser.AddScriptElement(Properties.Resources.underscore_js);
            this.statDetailsWebBrowser.AddScriptElement(Properties.Resources.knockout_3_0_0_debug_js);
            this.statDetailsWebBrowser.AddScriptElement(Properties.Resources.knockout_StringInterpolatingBindingProvider_js);
            this.statDetailsWebBrowser.AddScriptElement(Properties.Resources.ko_ninja_js);
            this.statDetailsWebBrowser.AddScriptElement(Properties.Resources.statblockHelpers_js);
            this.statDetailsWebBrowser.AddScriptElement(Properties.Resources.bindingHandlers_js);
        }

        private async Task<int?> AddFilesToStatsListAsync (CombatantType type) {
            string filter = type == CombatantType.Monster ? "Monster Files|*.monster" : "Character Files|*.dnd4e";
            OpenFileDialog dialog = new OpenFileDialog() {
                Filter = filter + "|All files (*.*)|*.*",
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = true,
                ValidateNames = true
            };
            DialogResult result = dialog.ShowDialog();

            if ((result != DialogResult.OK) || (dialog.FileNames.Length == 0)) {
                return null;
            }

            Stopwatch timer = Stopwatch.StartNew();
            int index = -1;
            IEnumerable<Combatant> added = null;
            if (type == CombatantType.Character) {
                added = await this.library.LoadCharactersFromFileAsync(dialog.FileNames);
            }
            else if (type == CombatantType.Monster) {
                added = await this.library.LoadMonstersFromFileAsync(dialog.FileNames);
            }
            timer.Stop();
            Trace.TraceInformation("Loading {0}s took {1}ms", type, timer.ElapsedMilliseconds);

            this.statsListBox.SuspendLayout();
            foreach (var combatant in added) {
                var old = this.statsListBox.Items
                                           .OfType<Combatant>()
                                           .Where(m => m.Handle == combatant.Handle);
                if (old.Any()) {
                    this.statsListBox.Items.Remove(old.Single());
                }
                index = this.statsListBox.Items.Add(combatant);
            }
            this.statsListBox.ResumeLayout();

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

                if (key == null) {
                    return;
                }

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
                Trace.TraceError(ex.ToString());
                System.Diagnostics.Debugger.Break();
            }
        }

        private void RenderCombatantDetails (Task<string> serializeTask) {
            if (serializeTask == null) {
                return;
            }

            // this will wait if the serialize is still running
            string json = serializeTask.Result;

            try {
                this.statDetailsWebBrowser.Document.InvokeScript(
                    "renderStatBlock",
                    new object[] { json }
                );
            }
            catch (System.Exception ex) {
                Trace.TraceError(ex.ToString());
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
                if (role == "Hero") {
                    query = query.Where(c => c is Character);
                }
                else {
                    query = query.Where(c => c is Monster);
                    query = query.Where(c => String.Equals(c.Role, role, StringComparison.OrdinalIgnoreCase));
                }
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
