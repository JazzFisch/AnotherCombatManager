using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DnD4e.CombatManager.Test.DockWindows;
using DnD4e.LibraryHelper.Character;
using DnD4e.LibraryHelper.Common;
using DnD4e.LibraryHelper.Monster;
using WeifenLuo.WinFormsUI.Docking;

namespace DnD4e.CombatManager.Test {
    // TODO: better handling of collapsed dock windows and expanding them
    //       E.g. hitting F4 if the properties window is already open
    // TODO: better property binding between windows
    public partial class LibraryForm : Form {
        #region Fields

        private CombatantListWindow<Character> charactersWindow = new CombatantListWindow<Character>() { Text = "Characters" };
        private CombatantListWindow<Monster> monstersWindow = new CombatantListWindow<Monster>() { Text = "Monsters" };
        private PropertiesWindow propertiesWindow = new PropertiesWindow();
        private StatblockWindow statblockWindow = new StatblockWindow();
        private IEnumerable<Combatant> selectedCombatants;

        #endregion

        #region Constructors

        public LibraryForm ()
            : this(null) {
        }

        public LibraryForm (Library library) {
            this.Library = library;
            InitializeComponent();

            this.charactersWindow.SelectionChanged += charactersWindow_SelectionChanged;
            this.monstersWindow.SelectionChanged += monstersWindow_SelectionChanged;

            // setup text search throttle
            //var textChanged = Observable.FromEventPattern(this.toolStripNameTextBox, "TextChanged").Select(x => ((ToolStripTextBox)x.Sender).Text);
            //textChanged.Throttle(TimeSpan.FromMilliseconds(300))
            //           .ObserveOn(SynchronizationContext.Current)
            //           .Subscribe(text => {
            //               this.SetCombatants();
            //           });
        }

        #endregion

        public Library Library { get; private set; }

        #region Event Handlers

        private async void LibraryForm_Load (object sender, EventArgs e) {
            // the general idea is to get off the UI thread as soon as possible
            // while we load various parts of the library in the background
            if (this.Library == null) {
                this.Library = await Library.OpenLibraryAsync();
            }

            this.UpdateCounts();

            this.charactersWindow.Combatants = this.Library.Characters;
            this.charactersWindow.Show(this.dockPanel, DockState.DockLeft);

            this.monstersWindow.Combatants = this.Library.Monsters;
            this.monstersWindow.Show(this.dockPanel, DockState.DockLeft);

            this.propertiesWindow.Show(this.dockPanel, DockState.DockRightAutoHide);
            this.statblockWindow.Show(this.dockPanel, DockState.Document);

            await this.Library.LoadRulesAsync();
            this.importFromCBToolStripMenuItem.Enabled = true;
        }

        private void LibraryForm_FormClosing (object sender, FormClosingEventArgs e) {
            this.closeToolStripMenuItem_Click(sender, e);
        }

        private void closeToolStripMenuItem_Click (object sender, EventArgs e) {
            if (this.Library != null) {
                this.Library.Dispose();
            }
            if (!(e is FormClosingEventArgs)) {
                this.Close();
            }
        }

        private void charactersWindowToolStripMenuItem_Click (object sender, EventArgs e) {
            this.charactersWindow.Show(this.dockPanel);
            this.ActivateDockWindow(this.charactersWindow);
        }

        void charactersWindow_SelectionChanged (object sender, CombatantsSelectionChangedEventArgs<Character> e) {
            if (e.Combatants.Count() == 1) {
                this.propertiesWindow.SelectedObject = e.Combatants.First();
                this.statblockWindow.Combatant = e.Combatants.First();
            }
            this.selectedCombatants = e.Combatants;
        }

        private async void importFromATToolStripMenuItem_Click (object sender, EventArgs e) {
            await this.AddFilesToStatsListAsync(CombatantType.Monster);
        }

        private async void importFromCBToolStripMenuItem_Click (object sender, EventArgs e) {
            await this.AddFilesToStatsListAsync(CombatantType.Character);
        }

        void monstersWindow_SelectionChanged (object sender, CombatantsSelectionChangedEventArgs<Monster> e) {
            if (e.Combatants.Count() == 1) {
                this.propertiesWindow.SelectedObject = e.Combatants.First();
                this.statblockWindow.Combatant = e.Combatants.First();
            }
            this.selectedCombatants = e.Combatants;
        }

        private void monstersWindowToolStripMenuItem_Click (object sender, EventArgs e) {
            this.monstersWindow.Show(this.dockPanel);
            this.ActivateDockWindow(this.monstersWindow);
        }

        private void propertiesToolStripMenuItem_Click (object sender, EventArgs e) {
            this.propertiesWindow.Show(this.dockPanel);
            this.ActivateDockWindow(this.propertiesWindow);
        }

        private void removeToolStripMenuItem_Click (object sender, EventArgs e) {
            if ((this.selectedCombatants == null) || !this.selectedCombatants.Any()) {
                return;
            }

            var type = this.selectedCombatants.First().CombatantType;
            switch (type) {
                case CombatantType.Character:
                    if (!this.charactersWindow.IsActivated) {
                        return;
                    }
                    break;

                case CombatantType.Monster:
                    if (!this.monstersWindow.IsActivated) {
                        return;
                    }
                    break;

                default:
                    return;
            }

            var prompt = String.Format(
                "Are you sure you want to delete the\nfollowing entries from the Library?\n{0}",
                String.Join("    \n", this.selectedCombatants.Select(c => c.Handle))
            );

            var result = MessageBox.Show(prompt, "Delete request", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result != DialogResult.OK) {
                return;
            }

            switch (type) {
                case CombatantType.Character:
                    this.RemoveCombatants<Character>(this.Library.Characters, this.selectedCombatants);
                    break;

                case CombatantType.Monster:
                    this.RemoveCombatants<Monster>(this.Library.Monsters, this.selectedCombatants);
                    break;
            }
            this.UpdateCounts();
        }

        private async void saveToolStripMenuItem_Click (object sender, EventArgs e) {
            await this.Library.FlushAsync();
        }

        private void statblockWindowToolStripMenuItem_Click (object sender, EventArgs e) {
            this.statblockWindow.Show(this.dockPanel);
        }

        #endregion

        #region Private Methods

        private void ActivateDockWindow (DockContent window) {
            if (!window.IsActivated) {
                window.PanelPane.Activate();
                window.Activate();
            }
        }

        private async Task AddFilesToStatsListAsync (CombatantType type) {
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
                return;
            }

            Stopwatch timer = Stopwatch.StartNew();
            IEnumerable<Combatant> added = null;
            if (type == CombatantType.Character) {
                added = await this.Library.ImportCharactersFromFileAsync(dialog.FileNames);
            }
            else if (type == CombatantType.Monster) {
                added = await this.Library.ImportMonstersFromFileAsync(dialog.FileNames);
            }
            timer.Stop();
            Trace.TraceInformation("Loading {0}s took {1}ms", type, timer.ElapsedMilliseconds);

            this.UpdateCounts();
        }

        //private int CalculateEncounterLevel (int partySize, int encounterXP) {
        //    if (encounterXP == 0 || partySize == 0) {
        //        return 0;
        //    }

        //    // TODO: create actual calculation rather conditional logic
        //    int perPC = encounterXP / partySize;
        //    if (perPC <= 112) { return 1; }
        //    else if (perPC <= 137) { return 2; }
        //    else if (perPC <= 162) { return 3; }
        //    else if (perPC <= 187) { return 4; }
        //    else if (perPC <= 225) { return 5; }

        //    else if (perPC <= 275) { return 6; }
        //    else if (perPC <= 325) { return 7; }
        //    else if (perPC <= 375) { return 8; }
        //    else if (perPC <= 425) { return 9; }
        //    else if (perPC <= 475) { return 10; }

        //    else if (perPC <= 650) { return 11; }
        //    else if (perPC <= 750) { return 12; }
        //    else if (perPC <= 900) { return 13; }
        //    else if (perPC <= 1100) { return 14; }
        //    else if (perPC <= 1300) { return 15; }

        //    else if (perPC <= 1500) { return 16; }
        //    else if (perPC <= 1800) { return 17; }
        //    else if (perPC <= 2200) { return 18; }
        //    else if (perPC <= 2600) { return 19; }
        //    else if (perPC <= 3000) { return 20; }

        //    else if (perPC <= 3675) { return 21; }
        //    else if (perPC <= 4625) { return 22; }
        //    else if (perPC <= 5575) { return 23; }
        //    else if (perPC <= 6525) { return 24; }
        //    else if (perPC <= 8000) { return 25; }

        //    else if (perPC <= 10000) { return 26; }
        //    else if (perPC <= 12000) { return 27; }
        //    else if (perPC <= 14000) { return 28; }
        //    else if (perPC <= 17000) { return 29; }
        //    else if (perPC <= 21000) { return 30; }

        //    else if (perPC <= 25000) { return 31; }
        //    else if (perPC <= 29000) { return 32; }
        //    else if (perPC <= 35000) { return 33; }
        //    else if (perPC <= 43000) { return 34; }
        //    else if (perPC <= 51000) { return 35; }

        //    else if (perPC <= 59000) { return 36; }
        //    else if (perPC <= 71000) { return 37; }
        //    else if (perPC <= 87000) { return 38; }
        //    else if (perPC <= 103000) { return 39; }
        //    else { return 40; }
        //}

        private void RemoveCombatants<T> (ObservableCombatantDictionary<T> combatants, IEnumerable<Combatant> victims) where T : Combatant {
            combatants.FireEvents = false;
            foreach (var victim in victims) {
                combatants.Remove(victim.Handle);
            }
            combatants.FireEvents = true;
            combatants.RaiseCollectionChanged(System.Collections.Specialized.NotifyCollectionChangedAction.Reset);
        }

        private void UpdateCounts () {
            this.characterCountToolStripStatusLabel.Text = this.Library.Characters.Count().ToString("#,0");
            this.monsterCountToolStripStatusLabel.Text = this.Library.Monsters.Count().ToString("#,0");
        }

        //private void UpdateXPTotals () {
        //    int xp = this.addToBattleListBox.Items.OfType<Monster>().Sum(m => m.Experience);
        //    this.totalXPTextBox.Text = xp.ToString("#,0");

        //    var characters = this.addToBattleListBox.Items.OfType<Character>().ToList();

        //    if (!characters.Any()) {
        //        this.xpLevelFor4TextBox.Text = this.xpLevelFor5TextBox.Text = this.xpLevelFor6TextBox.Text = "0";
        //        return;
        //    }

        //    this.xpLevelFor4TextBox.Text = this.CalculateEncounterLevel(4, xp).ToString("#,0");
        //    this.xpLevelFor5TextBox.Text = this.CalculateEncounterLevel(5, xp).ToString("#,0");
        //    this.xpLevelFor6TextBox.Text = this.CalculateEncounterLevel(6, xp).ToString("#,0");
        //}

        #endregion
    }
}
