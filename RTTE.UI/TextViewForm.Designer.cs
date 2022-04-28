
namespace RTTE.UI {
    partial class TextViewForm {
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
            this.textView1 = new RTTE.UI.TextView();
            this.SuspendLayout();
            // 
            // textView1
            // 
            this.textView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textView1.Location = new System.Drawing.Point(41, 50);
            this.textView1.Name = "textView1";
            this.textView1.Size = new System.Drawing.Size(524, 58);
            this.textView1.TabIndex = 0;
            this.textView1.Translator = null;
            // 
            // TextViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(596, 350);
            this.Controls.Add(this.textView1);
            this.Name = "TextViewForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RTTE ({0}) - Text Viewer & Translator";
            this.Load += new System.EventHandler(this.TextViewForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private TextView textView1;
    }
}