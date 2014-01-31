using System;
using MahApps.Metro.Controls;

namespace AnotherCM.WPF.ViewModels.Flyouts {
    public class SettingsViewModel : FlyoutBaseViewModel {

        public SettingsViewModel () {
            this.Header = "Settings";
            this.Position = Position.Right;
        }

        public void Cancel () {
            this.IsOpen = false;
        }

        public void Save () {

        }
    }
}