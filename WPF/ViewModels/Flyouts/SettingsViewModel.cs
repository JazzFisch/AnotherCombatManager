using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using AnotherCM.Library.Common;
using AnotherCM.WPF.Framework;
using MahApps.Metro.Controls;
using Microsoft.Win32;

namespace AnotherCM.WPF.ViewModels.Flyouts {
    [Export(typeof(ISettingsFlyout))]
    public class SettingsViewModel : FlyoutBaseViewModel, ISettingsFlyout {
        private ILibrary library;

        [ImportingConstructor]
        public SettingsViewModel (ILibrary library) {
            this.library = library;
            this.Header = "Settings";
            this.Position = Position.Right;
        }

        public void Cancel () {
            this.IsOpen = false;
        }

        public string FilePath {
            get {
                return this.library.FilePath;
            }
            set {
                if (!String.Equals(value, this.library.FilePath)) {
                    this.library.FilePath = value;
                    this.NotifyOfPropertyChange(() => this.FilePath);
                }
            }
        }

        public async Task AddCharacter () {
            await this.AddCombatantsAsync(RenderType.Character);
        }

        public async Task AddMonster () {
            await this.AddCombatantsAsync(RenderType.Monster);
        }

        public void Save () {
            this.library.Flush();
        }

        private async Task AddCombatantsAsync (RenderType type, string title = "Import") {
            string filter = type == RenderType.Monster ? "Monster Files|*.monster" : "Character Files|*.dnd4e";
            OpenFileDialog dialog = new OpenFileDialog() {
                Filter = filter + "|All files (*.*)|*.*",
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = true,
                Title = title,
                ValidateNames = true
            };

            var result = dialog.ShowDialog();
            if (!result.HasValue || !result.Value || (dialog.FileNames.Length == 0)) {
                return;
            }

            if (type == RenderType.Character) {
                await this.library.AddCharactersAsync(dialog.FileNames);
            }
            else if (type == RenderType.Monster) {
                await this.library.AddMonstersAsync(dialog.FileNames);
            }
        }
    }
}