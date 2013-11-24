using System;
using WeifenLuo.WinFormsUI.Docking;

namespace DnD4e.CombatManager.Test.DockWindows {
    public partial class PropertiesWindow : DockContent {
        public PropertiesWindow () {
            InitializeComponent();
        }

        public object SelectedObject {
            get {
                return this.propertyGrid.SelectedObject;
            }
            set {
                this.propertyGrid.SelectedObject = value;
            }
        }
    }
}
