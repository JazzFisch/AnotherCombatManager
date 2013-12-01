namespace DnD4e.CombatManager.Test.Controls {
    partial class SearchableObjectListView {
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent () {
            this.searchTextBox = new BCCL.UI.WinForms.SearchTextBox();
            this.objectListView = new BrightIdeasSoftware.ObjectListView();
            ((System.ComponentModel.ISupportInitialize)(this.objectListView)).BeginInit();
            this.SuspendLayout();
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
            this.searchTextBox.Size = new System.Drawing.Size(386, 20);
            this.searchTextBox.TabIndex = 0;
            // 
            // objectListView
            // 
            this.objectListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.objectListView.Location = new System.Drawing.Point(1, 24);
            this.objectListView.Name = "objectListView";
            this.objectListView.Size = new System.Drawing.Size(386, 456);
            this.objectListView.TabIndex = 1;
            this.objectListView.UseCompatibleStateImageBehavior = false;
            this.objectListView.View = System.Windows.Forms.View.Details;
            // 
            // SearchableObjectListView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.objectListView);
            this.Controls.Add(this.searchTextBox);
            this.Name = "SearchableObjectListView";
            this.Padding = new System.Windows.Forms.Padding(1, 1, 1, 0);
            this.Size = new System.Drawing.Size(388, 486);
            ((System.ComponentModel.ISupportInitialize)(this.objectListView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BCCL.UI.WinForms.SearchTextBox searchTextBox;
        private BrightIdeasSoftware.ObjectListView objectListView;

    }
}
