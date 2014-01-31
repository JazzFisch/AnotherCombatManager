using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using AnotherCM.WPF.Framework;
using Caliburn.Micro;

namespace AnotherCM.WPF.ViewModels {
    [Export(typeof(IShell))]
    public class ShellViewModel : Conductor<IScreen>.Collection.OneActive, IHaveDisplayName {
        private readonly IObservableCollection<FlyoutBaseViewModel> flyouts;
        private readonly Flyouts.SettingsViewModel settings;

        [ImportingConstructor]
        public ShellViewModel ([ImportMany]IEnumerable<IShellViewTab> tabs) {
            this.settings = new Flyouts.SettingsViewModel();

            // add all the flyouts
            this.flyouts = new BindableCollection<FlyoutBaseViewModel>();
            this.flyouts.Add(this.settings);

            // add the tabs
            this.Items.AddRange(tabs);
        }

        public IObservableCollection<FlyoutBaseViewModel> FlyoutsCollection {
            get { return this.flyouts; }
        }

        public override void CanClose (Action<bool> callback) {
            base.CanClose(callback);
        }

        public void OpenSettings () {
            this.settings.IsOpen = !this.settings.IsOpen;
        }

        protected override void OnInitialize () {
            this.DisplayName = "Jason Was Here";
            base.OnInitialize();
        }
    }
}