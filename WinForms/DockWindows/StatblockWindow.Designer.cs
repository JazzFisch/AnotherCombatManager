namespace AnotherCM.WinForms.DockWindows {
    partial class StatblockWindow {
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
            this.statblockControl = new AnotherCM.WinForms.Controls.StatblockControl();
            this.SuspendLayout();
            // 
            // statblockControl
            // 
            this.statblockControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statblockControl.Location = new System.Drawing.Point(0, 0);
            this.statblockControl.MinimumSize = new System.Drawing.Size(20, 20);
            this.statblockControl.Name = "statblockControl";
            this.statblockControl.RenderMethod = "renderStatBlock";
            this.statblockControl.SelectedObject = null;
            this.statblockControl.Size = new System.Drawing.Size(567, 457);
            this.statblockControl.TabIndex = 0;
            // 
            // StatblockWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(567, 457);
            this.Controls.Add(this.statblockControl);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "StatblockWindow";
            this.Text = "Statblock";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.StatblockControl statblockControl;

    }
}