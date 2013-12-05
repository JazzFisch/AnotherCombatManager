using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AnotherCM.Library.Common;
using AnotherCM.Library.Character;
using AnotherCM.Library.Monster;
using Microsoft.Win32;
using WeifenLuo.WinFormsUI.Docking;

namespace DnD4e.CombatManager.Test.DockWindows {
    public partial class StatblockWindow : DockContent {
        #region Constructors

        public StatblockWindow () {
            InitializeComponent();
        }

        #endregion

        #region Public Properties

        public Combatant Combatant {
            get {
                return this.statblockControl.SelectedObject as Combatant;
            }
            set {
                if (value == null) {
                    throw new ArgumentNullException("value");
                }
                this.Text = value.Name;
                this.statblockControl.SelectedObject = value;
            }
        }

        #endregion
    }
}
