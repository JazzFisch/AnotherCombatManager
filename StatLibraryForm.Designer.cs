namespace DnD4e.CombatManager.Test {
    partial class StatLibraryForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose (bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent () {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StatLibraryForm));
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.toolStripNameFilter = new System.Windows.Forms.ToolStrip();
            this.toolStripLabelName = new System.Windows.Forms.ToolStripLabel();
            this.toolStripNameTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripItemClearButton = new System.Windows.Forms.ToolStripButton();
            this.ToolStripAdditionalFilters = new System.Windows.Forms.ToolStrip();
            this.toolStripLevelsLabel = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLowLevelTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripToLabel = new System.Windows.Forms.ToolStripLabel();
            this.toolStripHighLevelTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripRoleComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripRoleLabel = new System.Windows.Forms.ToolStripLabel();
            this.statsListBox = new System.Windows.Forms.ListBox();
            this.toolStripStatListButtons = new System.Windows.Forms.ToolStrip();
            this.ToolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripStatListLoadATButon = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripStatListLoadCBButton = new System.Windows.Forms.ToolStripButton();
            this.ToolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripStatListDeleteButton = new System.Windows.Forms.ToolStripButton();
            this.ToolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.statDetailsWebBrowser = new System.Windows.Forms.WebBrowser();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.toolStripNameFilter.SuspendLayout();
            this.ToolStripAdditionalFilters.SuspendLayout();
            this.toolStripStatListButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.toolStripContainer1);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.statDetailsWebBrowser);
            this.splitContainer.Size = new System.Drawing.Size(763, 554);
            this.splitContainer.SplitterDistance = 318;
            this.splitContainer.TabIndex = 0;
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.toolStripNameFilter);
            this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.ToolStripAdditionalFilters);
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.statsListBox);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(318, 479);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(318, 554);
            this.toolStripContainer1.TabIndex = 0;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStripStatListButtons);
            // 
            // toolStripNameFilter
            // 
            this.toolStripNameFilter.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStripNameFilter.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripNameFilter.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabelName,
            this.toolStripNameTextBox,
            this.toolStripItemClearButton});
            this.toolStripNameFilter.Location = new System.Drawing.Point(0, 0);
            this.toolStripNameFilter.Name = "toolStripNameFilter";
            this.toolStripNameFilter.Size = new System.Drawing.Size(318, 25);
            this.toolStripNameFilter.Stretch = true;
            this.toolStripNameFilter.TabIndex = 1;
            this.toolStripNameFilter.Text = "X";
            // 
            // toolStripLabelName
            // 
            this.toolStripLabelName.Name = "toolStripLabelName";
            this.toolStripLabelName.Size = new System.Drawing.Size(42, 22);
            this.toolStripLabelName.Text = "Name:";
            // 
            // toolStripNameTextBox
            // 
            this.toolStripNameTextBox.Name = "toolStripNameTextBox";
            this.toolStripNameTextBox.Size = new System.Drawing.Size(200, 25);
            this.toolStripNameTextBox.ToolTipText = "Filter List by Name";
            // 
            // toolStripItemClearButton
            // 
            this.toolStripItemClearButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripItemClearButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripItemClearButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripItemClearButton.Name = "toolStripItemClearButton";
            this.toolStripItemClearButton.Size = new System.Drawing.Size(38, 22);
            this.toolStripItemClearButton.Text = "Clear";
            this.toolStripItemClearButton.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.toolStripItemClearButton.ToolTipText = "Clear Filter";
            // 
            // ToolStripAdditionalFilters
            // 
            this.ToolStripAdditionalFilters.Dock = System.Windows.Forms.DockStyle.None;
            this.ToolStripAdditionalFilters.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ToolStripAdditionalFilters.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLevelsLabel,
            this.toolStripLowLevelTextBox,
            this.toolStripToLabel,
            this.toolStripHighLevelTextBox,
            this.toolStripRoleComboBox,
            this.toolStripRoleLabel});
            this.ToolStripAdditionalFilters.Location = new System.Drawing.Point(0, 25);
            this.ToolStripAdditionalFilters.Name = "ToolStripAdditionalFilters";
            this.ToolStripAdditionalFilters.Size = new System.Drawing.Size(318, 25);
            this.ToolStripAdditionalFilters.Stretch = true;
            this.ToolStripAdditionalFilters.TabIndex = 2;
            // 
            // toolStripLevelsLabel
            // 
            this.toolStripLevelsLabel.Name = "toolStripLevelsLabel";
            this.toolStripLevelsLabel.Size = new System.Drawing.Size(42, 22);
            this.toolStripLevelsLabel.Text = "Levels:";
            // 
            // toolStripLowLevelTextBox
            // 
            this.toolStripLowLevelTextBox.Name = "toolStripLowLevelTextBox";
            this.toolStripLowLevelTextBox.Size = new System.Drawing.Size(30, 25);
            this.toolStripLowLevelTextBox.Text = "1";
            this.toolStripLowLevelTextBox.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolStripLowLevelTextBox.ToolTipText = "Filter List by Level";
            // 
            // toolStripToLabel
            // 
            this.toolStripToLabel.Name = "toolStripToLabel";
            this.toolStripToLabel.Size = new System.Drawing.Size(18, 22);
            this.toolStripToLabel.Text = "to";
            // 
            // toolStripHighLevelTextBox
            // 
            this.toolStripHighLevelTextBox.Name = "toolStripHighLevelTextBox";
            this.toolStripHighLevelTextBox.Size = new System.Drawing.Size(30, 25);
            this.toolStripHighLevelTextBox.Text = "40";
            this.toolStripHighLevelTextBox.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolStripHighLevelTextBox.ToolTipText = "Filter List by Level";
            // 
            // toolStripRoleComboBox
            // 
            this.toolStripRoleComboBox.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripRoleComboBox.Items.AddRange(new object[] {
            "",
            "Artillery",
            "Blaster",
            "Brute",
            "Controller",
            "Hero",
            "Lurker",
            "Minion",
            "Obstacle",
            "Puzzle",
            "Skirmisher",
            "Soldier",
            "Warder"});
            this.toolStripRoleComboBox.Name = "toolStripRoleComboBox";
            this.toolStripRoleComboBox.Size = new System.Drawing.Size(80, 25);
            // 
            // toolStripRoleLabel
            // 
            this.toolStripRoleLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripRoleLabel.Name = "toolStripRoleLabel";
            this.toolStripRoleLabel.Size = new System.Drawing.Size(33, 22);
            this.toolStripRoleLabel.Text = "Role:";
            // 
            // statsListBox
            // 
            this.statsListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statsListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statsListBox.FormattingEnabled = true;
            this.statsListBox.ItemHeight = 20;
            this.statsListBox.Location = new System.Drawing.Point(0, 0);
            this.statsListBox.Name = "statsListBox";
            this.statsListBox.ScrollAlwaysVisible = true;
            this.statsListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.statsListBox.Size = new System.Drawing.Size(318, 479);
            this.statsListBox.Sorted = true;
            this.statsListBox.TabIndex = 1;
            this.statsListBox.SelectedIndexChanged += new System.EventHandler(this.statsListBox_SelectedIndexChanged);
            this.statsListBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.statsListBox_KeyDown);
            // 
            // toolStripStatListButtons
            // 
            this.toolStripStatListButtons.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStripStatListButtons.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripStatListButtons.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripSeparator2,
            this.toolStripStatListLoadATButon,
            this.toolStripSeparator3,
            this.toolStripStatListLoadCBButton,
            this.ToolStripSeparator1,
            this.toolStripStatListDeleteButton,
            this.ToolStripSeparator7});
            this.toolStripStatListButtons.Location = new System.Drawing.Point(0, 0);
            this.toolStripStatListButtons.Name = "toolStripStatListButtons";
            this.toolStripStatListButtons.Size = new System.Drawing.Size(318, 25);
            this.toolStripStatListButtons.Stretch = true;
            this.toolStripStatListButtons.TabIndex = 1;
            // 
            // ToolStripSeparator2
            // 
            this.ToolStripSeparator2.Name = "ToolStripSeparator2";
            this.ToolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripStatListLoadATButon
            // 
            this.toolStripStatListLoadATButon.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatListLoadATButon.Image = ((System.Drawing.Image)(resources.GetObject("toolStripStatListLoadATButon.Image")));
            this.toolStripStatListLoadATButon.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripStatListLoadATButon.Name = "toolStripStatListLoadATButon";
            this.toolStripStatListLoadATButon.Size = new System.Drawing.Size(55, 22);
            this.toolStripStatListLoadATButon.Text = "Load AT";
            this.toolStripStatListLoadATButon.ToolTipText = "Load from Adventure Tools";
            this.toolStripStatListLoadATButon.Click += new System.EventHandler(this.toolStripStatListLoadATButon_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripStatListLoadCBButton
            // 
            this.toolStripStatListLoadCBButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatListLoadCBButton.Image = ((System.Drawing.Image)(resources.GetObject("toolStripStatListLoadCBButton.Image")));
            this.toolStripStatListLoadCBButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripStatListLoadCBButton.Name = "toolStripStatListLoadCBButton";
            this.toolStripStatListLoadCBButton.Size = new System.Drawing.Size(55, 22);
            this.toolStripStatListLoadCBButton.Text = "Load CB";
            this.toolStripStatListLoadCBButton.ToolTipText = "Load from Character Builder";
            this.toolStripStatListLoadCBButton.Click += new System.EventHandler(this.toolStripStatListLoadCBButton_Click);
            // 
            // ToolStripSeparator1
            // 
            this.ToolStripSeparator1.Name = "ToolStripSeparator1";
            this.ToolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripStatListDeleteButton
            // 
            this.toolStripStatListDeleteButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatListDeleteButton.Image = ((System.Drawing.Image)(resources.GetObject("toolStripStatListDeleteButton.Image")));
            this.toolStripStatListDeleteButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripStatListDeleteButton.Name = "toolStripStatListDeleteButton";
            this.toolStripStatListDeleteButton.Size = new System.Drawing.Size(44, 22);
            this.toolStripStatListDeleteButton.Text = "Delete";
            this.toolStripStatListDeleteButton.ToolTipText = "Delete selected entry";
            this.toolStripStatListDeleteButton.Click += new System.EventHandler(this.toolStripStatListDeleteButton_Click);
            // 
            // ToolStripSeparator7
            // 
            this.ToolStripSeparator7.Name = "ToolStripSeparator7";
            this.ToolStripSeparator7.Size = new System.Drawing.Size(6, 25);
            // 
            // statDetailsWebBrowser
            // 
            this.statDetailsWebBrowser.AllowNavigation = false;
            this.statDetailsWebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statDetailsWebBrowser.IsWebBrowserContextMenuEnabled = false;
            this.statDetailsWebBrowser.Location = new System.Drawing.Point(0, 0);
            this.statDetailsWebBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.statDetailsWebBrowser.Name = "statDetailsWebBrowser";
            this.statDetailsWebBrowser.Size = new System.Drawing.Size(441, 554);
            this.statDetailsWebBrowser.TabIndex = 0;
            this.statDetailsWebBrowser.WebBrowserShortcutsEnabled = false;
            // 
            // StatLibraryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(763, 554);
            this.Controls.Add(this.splitContainer);
            this.Name = "StatLibraryForm";
            this.Text = "StatLibraryForm";
            this.Load += new System.EventHandler(this.StatLibraryForm_Load);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.toolStripNameFilter.ResumeLayout(false);
            this.toolStripNameFilter.PerformLayout();
            this.ToolStripAdditionalFilters.ResumeLayout(false);
            this.ToolStripAdditionalFilters.PerformLayout();
            this.toolStripStatListButtons.ResumeLayout(false);
            this.toolStripStatListButtons.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        internal System.Windows.Forms.ToolStrip toolStripNameFilter;
        internal System.Windows.Forms.ToolStripLabel toolStripLabelName;
        internal System.Windows.Forms.ToolStripTextBox toolStripNameTextBox;
        internal System.Windows.Forms.ToolStripButton toolStripItemClearButton;
        internal System.Windows.Forms.ToolStrip ToolStripAdditionalFilters;
        internal System.Windows.Forms.ToolStripLabel toolStripLevelsLabel;
        internal System.Windows.Forms.ToolStripTextBox toolStripLowLevelTextBox;
        internal System.Windows.Forms.ToolStripLabel toolStripToLabel;
        internal System.Windows.Forms.ToolStripTextBox toolStripHighLevelTextBox;
        internal System.Windows.Forms.ToolStripComboBox toolStripRoleComboBox;
        internal System.Windows.Forms.ToolStripLabel toolStripRoleLabel;
        internal System.Windows.Forms.ListBox statsListBox;
        internal System.Windows.Forms.ToolStrip toolStripStatListButtons;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator2;
        internal System.Windows.Forms.ToolStripButton toolStripStatListDeleteButton;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator7;
        internal System.Windows.Forms.ToolStripButton toolStripStatListLoadATButon;
        internal System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        internal System.Windows.Forms.ToolStripButton toolStripStatListLoadCBButton;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator1;
        private System.Windows.Forms.WebBrowser statDetailsWebBrowser;
    }
}