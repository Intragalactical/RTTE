
namespace RTTE.UI {
    partial class SelectScreenAreaForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
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
        private void InitializeComponent() {
            this.SuspendLayout();
            // 
            // SelectScreenAreaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(579, 425);
            this.ControlBox = false;
            this.Cursor = System.Windows.Forms.Cursors.Cross;
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectScreenAreaForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.TopMost = true;
            this.Deactivate += new System.EventHandler(this.SelectScreenAreaForm_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SelectScreenAreaForm_FormClosing);
            this.Load += new System.EventHandler(this.SelectScreenAreaForm_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.SelectScreenAreaForm_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SelectScreenAreaForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SelectScreenAreaForm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SelectScreenAreaForm_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion
    }
}