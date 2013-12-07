namespace AnotherCM.WinForms.DockWindows {
    partial class EncounterDetailsWindow {
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
            this.addColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.removeColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.nameColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.levelColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.countColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.combatantTypeColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            ((System.ComponentModel.ISupportInitialize)(this.objectListView)).BeginInit();
            this.SuspendLayout();
            // 
            // objectListView
            // 
            this.objectListView.AllColumns.Add(this.addColumn);
            this.objectListView.AllColumns.Add(this.removeColumn);
            this.objectListView.AllColumns.Add(this.nameColumn);
            this.objectListView.AllColumns.Add(this.levelColumn);
            this.objectListView.AllColumns.Add(this.countColumn);
            this.objectListView.AllColumns.Add(this.combatantTypeColumn);
            this.objectListView.AllowColumnReorder = true;
            this.objectListView.AllowDrop = true;
            this.objectListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.addColumn,
            this.removeColumn,
            this.nameColumn,
            this.levelColumn,
            this.countColumn,
            this.combatantTypeColumn});
            this.objectListView.Cursor = System.Windows.Forms.Cursors.Default;
            this.objectListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectListView.FullRowSelect = true;
            this.objectListView.HeaderUsesThemes = false;
            this.objectListView.IsSimpleDropSink = true;
            this.objectListView.Location = new System.Drawing.Point(0, 0);
            this.objectListView.Name = "objectListView";
            this.objectListView.OwnerDraw = true;
            this.objectListView.SelectColumnsOnRightClickBehaviour = BrightIdeasSoftware.ObjectListView.ColumnSelectBehaviour.Submenu;
            this.objectListView.ShowCommandMenuOnRightClick = true;
            this.objectListView.ShowItemCountOnGroups = true;
            this.objectListView.Size = new System.Drawing.Size(706, 546);
            this.objectListView.TabIndex = 0;
            this.objectListView.UseCompatibleStateImageBehavior = false;
            this.objectListView.UseExplorerTheme = true;
            this.objectListView.UseFilterIndicator = true;
            this.objectListView.UseFiltering = true;
            this.objectListView.UseHotItem = true;
            this.objectListView.UseHyperlinks = true;
            this.objectListView.UseTranslucentHotItem = true;
            this.objectListView.UseTranslucentSelection = true;
            this.objectListView.View = System.Windows.Forms.View.Details;
            this.objectListView.HyperlinkClicked += new System.EventHandler<BrightIdeasSoftware.HyperlinkClickedEventArgs>(this.objectListView_HyperlinkClicked);
            this.objectListView.ModelCanDrop += new System.EventHandler<BrightIdeasSoftware.ModelDropEventArgs>(this.objectListView_ModelCanDrop);
            this.objectListView.ModelDropped += new System.EventHandler<BrightIdeasSoftware.ModelDropEventArgs>(this.objectListView_ModelDropped);
            this.objectListView.SelectionChanged += new System.EventHandler(this.objectListView_SelectionChanged);
            // 
            // addColumn
            // 
            this.addColumn.AspectToStringFormat = "";
            this.addColumn.CellPadding = null;
            this.addColumn.Groupable = false;
            this.addColumn.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.addColumn.Hideable = false;
            this.addColumn.Hyperlink = true;
            this.addColumn.IsEditable = false;
            this.addColumn.MaximumWidth = 14;
            this.addColumn.MinimumWidth = 14;
            this.addColumn.Searchable = false;
            this.addColumn.ShowTextInHeader = false;
            this.addColumn.Sortable = false;
            this.addColumn.Text = "+";
            this.addColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.addColumn.UseFiltering = false;
            this.addColumn.Width = 14;
            // 
            // removeColumn
            // 
            this.removeColumn.CellPadding = null;
            this.removeColumn.Groupable = false;
            this.removeColumn.Hideable = false;
            this.removeColumn.Hyperlink = true;
            this.removeColumn.IsEditable = false;
            this.removeColumn.MaximumWidth = 14;
            this.removeColumn.MinimumWidth = 14;
            this.removeColumn.Searchable = false;
            this.removeColumn.ShowTextInHeader = false;
            this.removeColumn.Sortable = false;
            this.removeColumn.Text = "-";
            this.removeColumn.UseFiltering = false;
            this.removeColumn.Width = 14;
            // 
            // nameColumn
            // 
            this.nameColumn.AspectName = "Name";
            this.nameColumn.CellPadding = null;
            this.nameColumn.FillsFreeSpace = true;
            this.nameColumn.MinimumWidth = 100;
            this.nameColumn.Text = "Name";
            this.nameColumn.Width = 100;
            // 
            // levelColumn
            // 
            this.levelColumn.AspectName = "Level";
            this.levelColumn.CellPadding = null;
            this.levelColumn.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.levelColumn.Text = "Level";
            this.levelColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.levelColumn.Width = 44;
            // 
            // countColumn
            // 
            this.countColumn.AspectName = "Count";
            this.countColumn.CellPadding = null;
            this.countColumn.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.countColumn.Text = "Count";
            this.countColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.countColumn.Width = 44;
            // 
            // combatantTypeColumn
            // 
            this.combatantTypeColumn.AspectName = "RenderType";
            this.combatantTypeColumn.CellPadding = null;
            this.combatantTypeColumn.Text = "Type";
            this.combatantTypeColumn.Width = 56;
            // 
            // EncounterDetailsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(706, 546);
            this.Controls.Add(this.objectListView);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "EncounterDetailsWindow";
            this.Text = "Encounter Details";
            ((System.ComponentModel.ISupportInitialize)(this.objectListView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BrightIdeasSoftware.ObjectListView objectListView;
        private BrightIdeasSoftware.OLVColumn nameColumn;
        private BrightIdeasSoftware.OLVColumn levelColumn;
        private BrightIdeasSoftware.OLVColumn countColumn;
        private BrightIdeasSoftware.OLVColumn combatantTypeColumn;
        private BrightIdeasSoftware.OLVColumn addColumn;
        private BrightIdeasSoftware.OLVColumn removeColumn;


    }
}