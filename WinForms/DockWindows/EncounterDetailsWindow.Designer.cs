namespace DnD4e.CombatManager.Test.DockWindows {
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
            this.nameColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.levelColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.countColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.combatantTypeColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            ((System.ComponentModel.ISupportInitialize)(this.objectListView)).BeginInit();
            this.SuspendLayout();
            // 
            // objectListView
            // 
            this.objectListView.AllColumns.Add(this.nameColumn);
            this.objectListView.AllColumns.Add(this.levelColumn);
            this.objectListView.AllColumns.Add(this.countColumn);
            this.objectListView.AllColumns.Add(this.combatantTypeColumn);
            this.objectListView.AllowDrop = true;
            this.objectListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameColumn,
            this.levelColumn,
            this.countColumn,
            this.combatantTypeColumn});
            this.objectListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectListView.IsSimpleDropSink = true;
            this.objectListView.Location = new System.Drawing.Point(0, 0);
            this.objectListView.Name = "objectListView";
            this.objectListView.Size = new System.Drawing.Size(706, 546);
            this.objectListView.TabIndex = 0;
            this.objectListView.UseCompatibleStateImageBehavior = false;
            this.objectListView.View = System.Windows.Forms.View.Details;
            this.objectListView.ModelCanDrop += new System.EventHandler<BrightIdeasSoftware.ModelDropEventArgs>(this.objectListView_ModelCanDrop);
            this.objectListView.ModelDropped += new System.EventHandler<BrightIdeasSoftware.ModelDropEventArgs>(this.objectListView_ModelDropped);
            // 
            // nameColumn
            // 
            this.nameColumn.AspectName = "Name";
            this.nameColumn.CellPadding = null;
            this.nameColumn.Text = "Name";
            // 
            // levelColumn
            // 
            this.levelColumn.AspectName = "Level";
            this.levelColumn.CellPadding = null;
            this.levelColumn.Text = "Level";
            // 
            // countColumn
            // 
            this.countColumn.AspectName = "Count";
            this.countColumn.CellPadding = null;
            this.countColumn.Text = "Count";
            // 
            // combatantTypeColumn
            // 
            this.combatantTypeColumn.AspectName = "Type";
            this.combatantTypeColumn.CellPadding = null;
            this.combatantTypeColumn.Text = "Type";
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


    }
}