using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DnD4e.CombatManager.Test.DockWindows;
using DnD4e.LibraryHelper.Character;
using DnD4e.LibraryHelper.Common;
using DnD4e.LibraryHelper.Encounter;
using DnD4e.LibraryHelper.Monster;
using WeifenLuo.WinFormsUI.Docking;

namespace DnD4e.CombatManager.Test {
    // TODO: put library file name in title (add option to specify / store library file name?)
    // TODO: lock all access to Library for true thread safe Open new Library?
    public partial class LibraryForm : Form {
        #region Fields

        private const string TitleFormat = "Library [{0}]";
        private readonly string DockPanelLayoutPath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");
        private CombatantListWindow<Character> charactersWindow = new CombatantListWindow<Character>() { Text = "Characters", HideOnClose = true };
        private EncountersWindow encountersWindow = new EncountersWindow() { HideOnClose = true };
        private CombatantListWindow<Monster> monstersWindow = new CombatantListWindow<Monster>() { Text = "Monsters", HideOnClose = true };
        private PropertiesWindow propertiesWindow = new PropertiesWindow() { HideOnClose = true };
        private EncounterDetailsWindow encounterDetailsWindow = new EncounterDetailsWindow() { HideOnClose = true };
        private StatblockWindow statblockWindow = new StatblockWindow() { HideOnClose = true };
        private IEnumerable<Combatant> selectedCombatants;
        private IEnumerable<Encounter> selectedEncounters;
        private bool saveLayout = true;

        #endregion

        #region Constructors

        public LibraryForm ()
            : this(null) {
        }

        public LibraryForm (Library library) {
            this.Library = library;
            InitializeComponent();

            this.charactersWindow.SelectionChanged += charactersWindow_SelectionChanged;
            this.encountersWindow.SelectionChanged += encountersWindow_SelectionChanged;
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
            if (File.Exists(DockPanelLayoutPath)) {
                // TODO: if any of our dock windows override GetPersistString, we'll need to reflect
                // that format here as well
                this.dockPanel.LoadFromXml(DockPanelLayoutPath, type => {
                    if (type == typeof(CombatantListWindow<Character>).ToString()) { return this.charactersWindow; }
                    else if (type == typeof(CombatantListWindow<Monster>).ToString()) { return this.monstersWindow; }
                    else if (type == typeof(EncountersWindow).ToString()) { return this.encountersWindow; }
                    else if (type == typeof(EncounterDetailsWindow).ToString()) { return this.encounterDetailsWindow; }
                    else if (type == typeof(PropertiesWindow).ToString()) { return this.propertiesWindow; }
                    else if (type == typeof(StatblockWindow).ToString()) { return this.statblockWindow; }
                    else { System.Diagnostics.Debugger.Break(); return null; }
                });
            }
            else {
                // default initial layout
                this.charactersWindow.Show(this.dockPanel, DockState.DockLeft);
                this.monstersWindow.Show(this.dockPanel, DockState.DockLeft);
                this.encountersWindow.Show(this.dockPanel, DockState.DockLeft);
                this.propertiesWindow.Show(this.dockPanel, DockState.DockRightAutoHide);
                this.statblockWindow.Show(this.dockPanel, DockState.Document);
            }

            // the general idea is to get off the UI thread as soon as possible
            // while we load various parts of the library in the background
            if (this.Library == null) {
                this.Library = await Library.OpenLibraryAsync();
            }
            this.UpdateCounts();

            await this.Library.LoadRulesAsync();
            this.importFromCBToolStripMenuItem.Enabled = true;
        }

        private void LibraryForm_FormClosing (object sender, FormClosingEventArgs e) {
            if (this.saveLayout) {
                this.dockPanel.SaveAsXml(DockPanelLayoutPath);
            }
            else {
                File.Delete(DockPanelLayoutPath);
            }

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
            this.ActivateDockWindow(this.charactersWindow);
        }

        private void charactersWindow_SelectionChanged (object sender, CombatantsSelectionChangedEventArgs<Character> e) {
            if (e.Combatants.Count() == 1) {
                this.propertiesWindow.SelectedObject = e.Combatants.First();
                this.statblockWindow.Combatant = e.Combatants.First();
            }
            this.selectedCombatants = e.Combatants;
        }

        private void encounterDetailsWindowToolStripMenuItem_Click (object sender, EventArgs e) {
            this.ActivateDockWindow(this.encounterDetailsWindow);
        }

        private void encountersWindow_SelectionChanged (object sender, EncountersSelectionChangedEventArgs e) {
            if (e.Encounters.Count() == 1) {
                this.encounterDetailsWindow.Encounter = e.Encounters.First();
                this.propertiesWindow.SelectedObject = e.Encounters.First();
            }
            this.selectedEncounters = e.Encounters;
        }

        private void encountersWindowToolStripMenuItem_Click (object sender, EventArgs e) {
            this.ActivateDockWindow(this.encountersWindow);
        }

        private async void importFromATToolStripMenuItem_Click (object sender, EventArgs e) {
            await this.AddFilesToStatsListAsync(CombatantType.Monster);
        }

        private async void importFromCBToolStripMenuItem_Click (object sender, EventArgs e) {
            await this.AddFilesToStatsListAsync(CombatantType.Character);
        }

        private void monstersWindow_SelectionChanged (object sender, CombatantsSelectionChangedEventArgs<Monster> e) {
            if (e.Combatants.Count() == 1) {
                this.propertiesWindow.SelectedObject = e.Combatants.First();
                this.statblockWindow.Combatant = e.Combatants.First();
            }
            this.selectedCombatants = e.Combatants;
        }

        private void monstersWindowToolStripMenuItem_Click (object sender, EventArgs e) {
            this.ActivateDockWindow(this.monstersWindow);
        }

        private async void openToolStripMenuItem_Click (object sender, EventArgs e) {
            OpenFileDialog dialog = new OpenFileDialog() {
                Filter = "Library Files|*.zip|All files (*.*)|*.*",
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = false,
                ValidateNames = true
            };
            DialogResult result = dialog.ShowDialog();
            if ((result != DialogResult.OK) || (dialog.FileNames.Length != 1)) {
                return;
            }

            var old = this.Library;
            var path = Path.GetDirectoryName(dialog.FileName);
            var file = Path.GetFileName(dialog.FileName);
            this.Library = await Library.OpenLibraryAsync(path, file);
            this.UpdateCounts();
            await old.FlushAsync();
        }

        private void propertiesToolStripMenuItem_Click (object sender, EventArgs e) {
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

        private async void saveAsToolStripMenuItem_Click (object sender, EventArgs e) {
            SaveFileDialog dialog = new SaveFileDialog() {
                Filter = "Library Files|*.zip|All files (*.*)|*.*",
                CheckPathExists = true,
                ValidateNames = true
            };
            DialogResult result = dialog.ShowDialog();
            if ((result != DialogResult.OK) || (dialog.FileNames.Length != 1)) {
                return;
            }

            await this.Library.SaveAsAsync(dialog.FileName);
            this.UpdateCounts();
        }

        private async void saveToolStripMenuItem_Click (object sender, EventArgs e) {
            await this.Library.FlushAsync();
        }

        private void statblockWindowToolStripMenuItem_Click (object sender, EventArgs e) {
            this.ActivateDockWindow(this.statblockWindow);
        }

        #endregion

        #region Private Methods

        private void ActivateDockWindow (DockContent window) {
            if (!window.IsActivated) {
                if (window.DockPanel == null) {
                    window.Show(this.dockPanel);
                }
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

        private void RemoveCombatants<T> (ObservableKeyedCollection<string, T> combatants, IEnumerable<Combatant> victims) where T : Combatant {
            foreach (var victim in victims) {
                combatants.Remove(victim.Handle);
            }
        }

        private void UpdateCounts () {
            this.Text = String.Format(TitleFormat, Path.GetFileName(this.Library.FileName));
            this.charactersWindow.Combatants = this.Library.Characters;
            this.monstersWindow.Combatants = this.Library.Monsters;
            this.encountersWindow.Encounters = this.Library.Encounters;

            this.characterCountToolStripStatusLabel.Text = this.Library.Characters.Count().ToString("#,0");
            this.monsterCountToolStripStatusLabel.Text = this.Library.Monsters.Count().ToString("#,0");
        }

        #endregion
    }
}
