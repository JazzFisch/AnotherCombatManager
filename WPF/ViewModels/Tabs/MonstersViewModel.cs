using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Windows.Data;
using AnotherCM.Library.Monster;
using AnotherCM.WPF.Framework;
using Caliburn.Micro;

namespace AnotherCM.WPF.ViewModels.Tabs {
    [Export(typeof(IShellViewTab))]
    public class MonstersViewModel : TabsBaseViewModel, IShellViewTab {
        private IEventAggregator eventAggregator;
        private ICollectionView items;
        private ILibrary library;
        private Monster selectedItem;

        [ImportingConstructor]
        public MonstersViewModel (ILibrary library, IEventAggregator eventAggregator) {
            this.DisplayName = "monsters";
            this.eventAggregator = eventAggregator;
            this.library = library;
            this.items = CollectionViewSource.GetDefaultView(this.library.Monsters);

            this.items.Filter = obj => {
                return true;
            };

            //var role = new PropertyGroupDescription("Role");
            //var level = new PropertyGroupDescription("Level");
            //this.items.GroupDescriptions.Add(role);
            //this.items.GroupDescriptions.Add(level);

            //var root = @"C:\Users\jasfi_000\SkyDrive\Games\Dungeons and Dragons 4e\Monsters\";
            //var monsterFiles = new string[] {
            //    root + "abyssal kraken.monster",
            //    root + "adept of orcus.monster",
            //    root + "adult silver dragon.monster",
            //    root + "beholder.monster",
            //    root + "king's dark lantern.monster",
            //    root + "kobold slinger.monster",
            //    root + "wererat.monster",
            //    root + "vampire lord (human rogue).monster",
            //    root + "storm titan thunder tempest.monster",
            //    root + "tempest wisp.monster",
            //};

            //Library.Common.Library.OpenLibraryAsync().ContinueWith(task => {
            //    this.library = task.Result;
            //    this.Source.AddRange(this.library.Monsters);
            //    //this.library.ImportMonstersFromFileAsync(monsterFiles);
            //});
        }

        public ICollectionView Items {
            get {
                return this.items;
            }
            set {
                this.items = value;
                this.NotifyOfPropertyChange(() => this.Items);
            }
        }

        public Monster SelectedItem {
            get { return this.selectedItem; }
            set {
                if (value == null) {
                    return;
                }
                this.selectedItem = value;
                this.eventAggregator.PublishOnBackgroundThread(value);
                NotifyOfPropertyChange(() => SelectedItem);
            }
        }

        protected override void OnDeactivate (bool close) {
            //this.library.Flush();
            base.OnDeactivate(close);
        }
    }
}