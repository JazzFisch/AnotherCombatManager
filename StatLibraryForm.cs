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
    public partial class LibraryForm : Form {
        #region Fields

        public static readonly Uri CompendiumBaseUrl = new Uri("https://www.wizards.com/dndinsider/compendium/display.aspx");
        private const string LoginPath = "/dndinsider/compendium/login.aspx";
        private const string WebBrowserEmulationPath = @"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION";
        private static readonly CultureInfo UICulture = Thread.CurrentThread.CurrentUICulture;
        private CombatantType combatantType = CombatantType.Invalid;
        //private string compendiumCookies;
        private int lowLevel = 1;
        private int highLevel = 40;
        private Library library;

        #endregion

        #region Constructors

        public LibraryForm () : this(null) {

        }

        public LibraryForm (Library library) {
            // fix IE rendering modes... *sigh*
            // perform prior to any other initialization
            this.SetBrowserRenderingRegistryKeys(addKeys: true);
            this.library = library;

            InitializeComponent();
            this.libraryListBox.Items.Add("Please wait, loading the goodness...");
            this.toolStripLowLevelTextBox.Text = lowLevel.ToString();
            this.toolStripHighLevelTextBox.Text = highLevel.ToString();

            // setup text search throttle
            var textChanged = Observable.FromEventPattern(this.toolStripNameTextBox, "TextChanged").Select(x => ((ToolStripTextBox)x.Sender).Text);
            textChanged.Throttle(TimeSpan.FromMilliseconds(300))
                       .ObserveOn(SynchronizationContext.Current)
                       .Subscribe(text => {
                           this.SetCombatants();
                       });
        }

        #endregion

        #region Properties

        public IEnumerable<Combatant> Combatants {
            get {
                return this.addToBattleListBox.Items.OfType<Combatant>();
            }
        }

        #endregion

        #region Event Handlers

        private async void LibraryForm_Load (object sender, EventArgs e) {
            // the general idea is to get off the UI thread as soon as possible
            // while we load various parts of the library in the background
            if (this.library == null) {
                this.library = await Library.OpenLibraryAsync();
            }
            SetCombatants();

            await this.library.LoadRulesAsync();
            this.toolStripStatListLoadCBButton.Enabled = true;
        }

        private void LibraryForm_FormClosing (object sender, FormClosingEventArgs e) {
            // remove the browser rendering keys
            this.SetBrowserRenderingRegistryKeys(addKeys: false);
            this.library.Dispose();
        }

        private void combatantDetailsWebBrowser_CompendiumCompleted (object sender, WebBrowserDocumentCompletedEventArgs e) {
            //this.statDetailsWebBrowser.AllowNavigation = false;
            //this.statDetailsWebBrowser.DocumentCompleted -= this.statDetailsWebBrowser_CompendiumCompleted;
            //var url = this.statDetailsWebBrowser.Url;
            //if (!url.LocalPath.Equals(LoginPath, StringComparison.OrdinalIgnoreCase)) {
            //    this.compendiumCookies = this.statDetailsWebBrowser.Document.Cookie;
            //    this.library.Add(this.statDetailsWebBrowser.DocumentText, url);
            //    this.SetCombatants();
            //}
            //else {
            //    if (this.statDetailsWebBrowser.EncryptionLevel == WebBrowserEncryptionLevel.Insecure) {
            //        // why does wizards redirect to an insecure url?!
            //        // re-redirect to a secure login page... if you can believe this
            //        // this makes our code more secure than theirs... *sigh*
            //        this.statDetailsWebBrowser.Navigate(String.Format(
            //            "https://{0}{1}{2}",
            //            url.DnsSafeHost,
            //            url.LocalPath,
            //            url.Query
            //        ));
            //    }
            //    this.statDetailsWebBrowser.AllowNavigation = true;
            //    this.statDetailsWebBrowser.DocumentCompleted += this.statDetailsWebBrowser_CompendiumCompleted;
            //}
        }

        private void libraryListBox_KeyDown (object sender, KeyEventArgs e) {
            if (e.Control && (e.KeyCode == Keys.A)) {
                this.libraryListBox.BeginUpdate();
                var old = this.libraryListBox.SelectionMode;
                for (int i = 0; i < this.libraryListBox.Items.Count; i++) {
                    this.libraryListBox.SetSelected(i, true);
                }
                this.libraryListBox.SelectionMode = old;
                this.libraryListBox.EndUpdate();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void libraryListBox_SelectedIndexChanged (object sender, EventArgs e) {
            Combatant combatant = this.libraryListBox.SelectedItem as Combatant;
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
                this.libraryListBox.ClearSelected();
                this.libraryListBox.SelectedIndex = index.Value;
            }
        }

        private async void toolStripStatListLoadCBButton_Click (object sender, EventArgs e) {
            int? index = await this.AddFilesToStatsListAsync(CombatantType.Character);
            if (index.HasValue) {
                this.libraryListBox.ClearSelected();
                this.libraryListBox.SelectedIndex = index.Value;
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

        private void toolStripStatListRemoveButton_Click (object sender, EventArgs e) {
            if (this.libraryListBox.SelectedIndices.Count == 0) {
                return;
            }

            var selected = this.libraryListBox.SelectedItems.OfType<Combatant>().ToList();
            var prompt = String.Format(
                "Are you sure you want to delete the\nfollowing entries from the Library?\n{0}",
                String.Join("    \n", selected.Select(c => c.Handle))
            );

            var result = MessageBox.Show(prompt, "Delete request", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result != DialogResult.OK) {
                return;
            }

            foreach (var combatant in selected) {
                this.library.Remove(combatant);
            }
            this.libraryListBox.SelectedIndices.Clear();
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

        private void toolStripAddToBattleButton_Click (object sender, EventArgs e) {
            var monsters = this.libraryListBox.SelectedItems.OfType<Monster>().ToArray();
            if (monsters.Length > 0) {
                this.addToBattleListBox.Items.AddRange(monsters);
            }
            
            // characters overwrite, not add
            var characters = this.libraryListBox.SelectedItems.OfType<Character>();
            foreach (var character in characters) {
                var matches = from c in this.addToBattleListBox.Items.OfType<Character>()
                              where c.Handle == character.Handle
                              select c;

                if (matches.Any()) {
                    this.addToBattleListBox.Items.Remove(matches.Single());
                }

                this.addToBattleListBox.Items.Add(character);
            }

            this.UpdateXPTotals();
        }

        private void toolStripRemoveFromBattleButton_Click (object sender, EventArgs e) {
            if (this.addToBattleListBox.SelectedIndices.Count == 0) {
                return;
            }

            var selected = this.addToBattleListBox.SelectedItems.OfType<Combatant>().ToList();
            foreach (var combatant in selected) {
                this.addToBattleListBox.Items.Remove(combatant);
            }

            this.addToBattleListBox.SelectedIndices.Clear();
            this.UpdateXPTotals();
        }

        #endregion

        #region Private Methods

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

            this.libraryListBox.SuspendLayout();
            foreach (var combatant in added) {
                var old = this.libraryListBox.Items
                                           .OfType<Combatant>()
                                           .Where(m => m.Handle == combatant.Handle);
                if (old.Any()) {
                    this.libraryListBox.Items.Remove(old.Single());
                }
                index = this.libraryListBox.Items.Add(combatant);
            }
            this.libraryListBox.ResumeLayout();

            if (dialog.FileNames.Length == 1) {
                return index;
            }
            else {
                return null;
            }
        }

        private int CalculateEncounterLevel (int partySize, int encounterXP) {
            if (encounterXP == 0 || partySize == 0) {
                return 0;
            }

            // TODO: create actual calculation rather conditional logic
            int perPC = encounterXP / partySize;
            if (perPC <= 112) { return 1; }
            else if (perPC <= 137) { return 2; }
            else if (perPC <= 162) { return 3; }
            else if (perPC <= 187) { return 4; }
            else if (perPC <= 225) { return 5; }

            else if (perPC <= 275) { return 6; }
            else if (perPC <= 325) { return 7; }
            else if (perPC <= 375) { return 8; }
            else if (perPC <= 425) { return 9; }
            else if (perPC <= 475) { return 10; }

            else if (perPC <= 650) { return 11; }
            else if (perPC <= 750) { return 12; }
            else if (perPC <= 900) { return 13; }
            else if (perPC <= 1100) { return 14; }
            else if (perPC <= 1300) { return 15; }

            else if (perPC <= 1500) { return 16; }
            else if (perPC <= 1800) { return 17; }
            else if (perPC <= 2200) { return 18; }
            else if (perPC <= 2600) { return 19; }
            else if (perPC <= 3000) { return 20; }

            else if (perPC <= 3675) { return 21; }
            else if (perPC <= 4625) { return 22; }
            else if (perPC <= 5575) { return 23; }
            else if (perPC <= 6525) { return 24; }
            else if (perPC <= 8000) { return 25; }

            else if (perPC <= 10000) { return 26; }
            else if (perPC <= 12000) { return 27; }
            else if (perPC <= 14000) { return 28; }
            else if (perPC <= 17000) { return 29; }
            else if (perPC <= 21000) { return 30; }

            else if (perPC <= 25000) { return 31; }
            else if (perPC <= 29000) { return 32; }
            else if (perPC <= 35000) { return 33; }
            else if (perPC <= 43000) { return 34; }
            else if (perPC <= 51000) { return 35; }

            else if (perPC <= 59000) { return 36; }
            else if (perPC <= 71000) { return 37; }
            else if (perPC <= 87000) { return 38; }
            else if (perPC <= 103000) { return 39; }
            else { return 40; }
        }

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
                    this.combatantDetailsWebBrowser.AddScriptElement(Properties.Resources.characterStatblock_js);
                    this.RenderCombatantDetails(serializeTask);
                };
            }
            else if (combatant is Monster) {
                html = Properties.Resources.monsterStatblock_html;
                completedHandler = (a, b) => {
                    this.StopListeningAndAddCommonHtmlElements(completedHandler);
                    this.combatantDetailsWebBrowser.AddScriptElement(Properties.Resources.monsterStatblock_js);
                    this.RenderCombatantDetails(serializeTask);
                };
            }
            else if (combatant is Trap) {
                html = Properties.Resources.trapStatblock_html;
                completedHandler = (a, b) => {
                    this.StopListeningAndAddCommonHtmlElements(completedHandler);
                    this.combatantDetailsWebBrowser.AddScriptElement(Properties.Resources.trapStatblock_js);
                    this.RenderCombatantDetails(serializeTask);
                };
            }
            else {
                return;
            }

            this.combatantDetailsWebBrowser.AllowNavigation = true;
            this.combatantDetailsWebBrowser.DocumentText = html;
            this.combatantDetailsWebBrowser.DocumentCompleted += completedHandler;
        }

        private void RenderCombatantDetails (Task<string> serializeTask) {
            if (serializeTask == null) {
                return;
            }

            // this will wait if the serialize is still running
            string json = serializeTask.Result;

            try {
                this.combatantDetailsWebBrowser.Document.InvokeScript(
                    "renderStatBlock",
                    new object[] { json }
                );
            }
            catch (System.Exception ex) {
                Trace.TraceError(ex.ToString());
                System.Diagnostics.Debugger.Break();
            }
        }

        private void SetBrowserRenderingRegistryKeys (bool addKeys) {
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
            this.libraryListBox.BeginUpdate();
            this.libraryListBox.Items.Clear();
            this.libraryListBox.Items.AddRange(matches.ToArray());
            this.libraryListBox.EndUpdate();
        }

        private void StopListeningAndAddCommonHtmlElements (WebBrowserDocumentCompletedEventHandler completedHandler) {
            // ordering of the following is IMPORTANT
            // stop listening
            this.combatantDetailsWebBrowser.AllowNavigation = false;
            this.combatantDetailsWebBrowser.DocumentCompleted -= completedHandler;

            // load our css in
            this.combatantDetailsWebBrowser.AddStyleSheet(Properties.Resources.statblock_css);

            // load our javascript in
            this.combatantDetailsWebBrowser.AddScriptElement(Properties.Resources.modernizr_2_6_2_js);
            this.combatantDetailsWebBrowser.AddScriptElement(Properties.Resources.underscore_js);
            this.combatantDetailsWebBrowser.AddScriptElement(Properties.Resources.knockout_3_0_0_debug_js);
            this.combatantDetailsWebBrowser.AddScriptElement(Properties.Resources.knockout_StringInterpolatingBindingProvider_js);
            this.combatantDetailsWebBrowser.AddScriptElement(Properties.Resources.ko_ninja_js);
            this.combatantDetailsWebBrowser.AddScriptElement(Properties.Resources.statblockHelpers_js);
            this.combatantDetailsWebBrowser.AddScriptElement(Properties.Resources.bindingHandlers_js);
        }

        private void UpdateXPTotals () {
            int xp = this.addToBattleListBox.Items.OfType<Monster>().Sum(m => m.Experience);
            this.totalXPTextBox.Text = xp.ToString("#,0");

            var characters = this.addToBattleListBox.Items.OfType<Character>().ToList();

            if (!characters.Any()) {
                this.xpLevelFor4TextBox.Text = this.xpLevelFor5TextBox.Text = this.xpLevelFor6TextBox.Text = "0";
                return;
            }

            this.xpLevelFor4TextBox.Text = this.CalculateEncounterLevel(4, xp).ToString("#,0");
            this.xpLevelFor5TextBox.Text = this.CalculateEncounterLevel(5, xp).ToString("#,0");
            this.xpLevelFor6TextBox.Text = this.CalculateEncounterLevel(6, xp).ToString("#,0");
        }

        #endregion
    }
}
