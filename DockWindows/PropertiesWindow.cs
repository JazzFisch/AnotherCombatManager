using System;
using System.Reactive.Linq;
using System.Threading;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace DnD4e.CombatManager.Test.DockWindows {
    public partial class PropertiesWindow : DockContent {
        public PropertiesWindow () {
            InitializeComponent();

            // setup text search throttle
            var textChanged = Observable.FromEventPattern(this.searchTextBox, "TextChanged").Select(x => ((TextBox)x.Sender).Text);
            textChanged.Throttle(TimeSpan.FromMilliseconds(300))
                       .ObserveOn(SynchronizationContext.Current)
                       .Subscribe(text => {
                           this.Search(text);
                       });
        }

        public object SelectedObject {
            get {
                return this.propertyGrid.SelectedObject;
            }
            set {
                this.propertyGrid.SelectedObject = value;
            }
        }

        private void searchTextBox_Cleared (object sender, EventArgs e) {
            // remove filter
        }

        private void Search (string text) {
        }
    }
}
