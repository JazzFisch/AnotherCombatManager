using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using AnotherCM.WPF.Framework;
using Caliburn.Micro;

namespace AnotherCM.WPF.ViewModels {
    [Export(typeof(IShell))]
    public class ShellViewModel : Conductor<IScreen>.Collection.OneActive, IHaveDisplayName {
        private readonly IObservableCollection<IFlyout> flyouts;
        private readonly ISettingsFlyout settings;

        [ImportingConstructor]
        public ShellViewModel ([ImportMany]IEnumerable<IShellViewTab> tabs, [Import]ISettingsFlyout settings) {
            this.settings = settings;

            // add all the flyouts
            this.flyouts = new BindableCollection<IFlyout>();
            this.flyouts.Add(this.settings);

            // add the tabs
            this.Items.AddRange(tabs);
        }

        public IObservableCollection<IFlyout> FlyoutsCollection {
            get { 
                return this.flyouts; 
            }
        }

        public override void CanClose (Action<bool> callback) {
            base.CanClose(callback);
        }

        public void OpenAbout () {
            var view = this.GetView() as Views.ShellView;
            var dialog = new PropertyTools.Wpf.AboutDialog(view);
            dialog.ShowDialog();
        }

        public void OpenSettings () {
            this.settings.IsOpen = !this.settings.IsOpen;
        }
        protected override void OnInitialize () {
            this.DisplayName = "Library - Another Combat Manager";
            base.OnInitialize();
        }
    }
}