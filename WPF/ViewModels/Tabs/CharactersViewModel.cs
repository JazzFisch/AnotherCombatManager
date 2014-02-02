using System;
using System.ComponentModel.Composition;
using AnotherCM.Library.Character;
using AnotherCM.WPF.Framework;
using Caliburn.Micro;

namespace AnotherCM.WPF.ViewModels.Tabs {
    [Export(typeof(IShellViewTab))]
    public class CharactersViewModel : TabsBaseViewModel, IShellViewTab {
        private IEventAggregator eventAggregator;
        private IObservableCollection<Character> items;
        private ILibrary library;
        private Character selectedItem;

        [ImportingConstructor]
        public CharactersViewModel (ILibrary library, IEventAggregator eventAggregator) {
            this.DisplayName = "characters";
            this.eventAggregator = eventAggregator;
            this.library = library;
            this.items = this.library.Characters;
        }

        public IObservableCollection<Character> Items {
            get {
                return this.items;
            }
            set {
                this.items = value;
                this.NotifyOfPropertyChange(() => this.Items);
            }
        }

        public Character SelectedItem {
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
            this.library.Flush();
            base.OnDeactivate(close);
        }
    }
}