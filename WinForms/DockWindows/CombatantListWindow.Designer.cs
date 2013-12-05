namespace DnD4e.CombatManager.Test.DockWindows {
    partial class CombatantListWindow<T> {
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
            this.objectListView = new BrightIdeasSoftware.ObjectListView();
            this.nameColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.levelColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.roleColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.searchTextBox = new BCCL.UI.WinForms.SearchTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.objectListView)).BeginInit();
            this.SuspendLayout();
            // 
            // objectListView
            // 
            this.objectListView.AllColumns.Add(this.nameColumn);
            this.objectListView.AllColumns.Add(this.levelColumn);
            this.objectListView.AllColumns.Add(this.roleColumn);
            this.objectListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.objectListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameColumn,
            this.levelColumn,
            this.roleColumn});
            this.objectListView.Cursor = System.Windows.Forms.Cursors.Default;
            this.objectListView.EmptyListMsg = "Loading the awesome...";
            this.objectListView.FullRowSelect = true;
            this.objectListView.HeaderUsesThemes = false;
            this.objectListView.IsSimpleDragSource = true;
            this.objectListView.Location = new System.Drawing.Point(1, 25);
            this.objectListView.Name = "objectListView";
            this.objectListView.OwnerDraw = true;
            this.objectListView.SelectColumnsOnRightClickBehaviour = BrightIdeasSoftware.ObjectListView.ColumnSelectBehaviour.Submenu;
            this.objectListView.ShowCommandMenuOnRightClick = true;
            this.objectListView.ShowItemCountOnGroups = true;
            this.objectListView.Size = new System.Drawing.Size(452, 460);
            this.objectListView.TabIndex = 1;
            this.objectListView.TintSortColumn = true;
            this.objectListView.UseCompatibleStateImageBehavior = false;
            this.objectListView.UseExplorerTheme = true;
            this.objectListView.UseFilterIndicator = true;
            this.objectListView.UseFiltering = true;
            this.objectListView.UseHotItem = true;
            this.objectListView.UseTranslucentHotItem = true;
            this.objectListView.UseTranslucentSelection = true;
            this.objectListView.View = System.Windows.Forms.View.Details;
            this.objectListView.SelectionChanged += new System.EventHandler(this.objectListView_SelectionChanged);
            // 
            // nameColumn
            // 
            this.nameColumn.AspectName = "Name";
            this.nameColumn.CellPadding = null;
            this.nameColumn.FillsFreeSpace = true;
            this.nameColumn.Hideable = false;
            this.nameColumn.IsEditable = false;
            this.nameColumn.Text = "Name";
            this.nameColumn.UseInitialLetterForGroup = true;
            this.nameColumn.Width = 96;
            // 
            // levelColumn
            // 
            this.levelColumn.AspectName = "Level";
            this.levelColumn.CellPadding = null;
            this.levelColumn.IsEditable = false;
            this.levelColumn.Text = "Level";
            this.levelColumn.Width = 50;
            // 
            // roleColumn
            // 
            this.roleColumn.AspectName = "Role";
            this.roleColumn.CellPadding = null;
            this.roleColumn.IsEditable = false;
            this.roleColumn.Text = "Role";
            this.roleColumn.Width = 80;
            // 
            // searchTextBox
            // 
            this.searchTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.searchTextBox.IconColorBase = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(69)))), ((int)(((byte)(114)))));
            this.searchTextBox.IconColorHighlight = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(137)))), ((int)(((byte)(193)))));
            this.searchTextBox.Location = new System.Drawing.Point(1, 1);
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.Size = new System.Drawing.Size(452, 20);
            this.searchTextBox.TabIndex = 0;
            this.searchTextBox.Cleared += new System.EventHandler(this.searchTextBox_Cleared);
            // 
            // CombatantListWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(453, 485);
            this.Controls.Add(this.searchTextBox);
            this.Controls.Add(this.objectListView);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "CombatantListWindow";
            this.Text = "CombatantListWindow";
            ((System.ComponentModel.ISupportInitialize)(this.objectListView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BrightIdeasSoftware.ObjectListView objectListView;
        private BrightIdeasSoftware.OLVColumn nameColumn;
        private BrightIdeasSoftware.OLVColumn levelColumn;
        private BrightIdeasSoftware.OLVColumn roleColumn;
        private BCCL.UI.WinForms.SearchTextBox searchTextBox;
    }
}