namespace RTTE.UI {
    partial class ProcessSelectForm {
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProcessSelectForm));
            this.ProcessDataGrid = new RTTE.UI.DoubleBufferedDataGridView();
            this.PanelRedIndicator = new System.Windows.Forms.Panel();
            this.LabelRedIndicator = new System.Windows.Forms.Label();
            this.LabelDarkGrayIndicator = new System.Windows.Forms.Label();
            this.PanelDarkGrayIndicator = new System.Windows.Forms.Panel();
            this.FilterTextBox = new System.Windows.Forms.TextBox();
            this.ButtonSelectAndContinue = new System.Windows.Forms.Button();
            this.LabelGreenIndicator = new System.Windows.Forms.Label();
            this.PanelGreenIndicator = new System.Windows.Forms.Panel();
            this.LabelWhiteIndicator = new System.Windows.Forms.Label();
            this.PanelWhiteIndicator = new System.Windows.Forms.Panel();
            this.ButtonClose = new System.Windows.Forms.Button();
            this.HideInvalidCheckBox = new System.Windows.Forms.CheckBox();
            this.HideWindowlessCheckBox = new System.Windows.Forms.CheckBox();
            this.HelpTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.MenuStripMain = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.ProcessDataGrid)).BeginInit();
            this.MenuStripMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // ProcessDataGrid
            // 
            this.ProcessDataGrid.AllowUserToAddRows = false;
            this.ProcessDataGrid.AllowUserToDeleteRows = false;
            this.ProcessDataGrid.AllowUserToOrderColumns = true;
            this.ProcessDataGrid.AllowUserToResizeRows = false;
            this.ProcessDataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProcessDataGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.ProcessDataGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.ProcessDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ProcessDataGrid.Location = new System.Drawing.Point(12, 65);
            this.ProcessDataGrid.MultiSelect = false;
            this.ProcessDataGrid.Name = "ProcessDataGrid";
            this.ProcessDataGrid.ReadOnly = true;
            this.ProcessDataGrid.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ProcessDataGrid.RowHeadersVisible = false;
            this.ProcessDataGrid.RowTemplate.Height = 25;
            this.ProcessDataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ProcessDataGrid.ShowEditingIcon = false;
            this.ProcessDataGrid.Size = new System.Drawing.Size(648, 453);
            this.ProcessDataGrid.TabIndex = 0;
            this.ProcessDataGrid.TabStop = false;
            this.ProcessDataGrid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ProcessDataGrid_CellDoubleClick);
            this.ProcessDataGrid.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.ProcessDataGrid_RowsAdded);
            this.ProcessDataGrid.SelectionChanged += new System.EventHandler(this.ProcessDataGrid_SelectionChanged);
            // 
            // PanelRedIndicator
            // 
            this.PanelRedIndicator.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.PanelRedIndicator.BackColor = System.Drawing.Color.Red;
            this.PanelRedIndicator.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelRedIndicator.Location = new System.Drawing.Point(12, 528);
            this.PanelRedIndicator.Name = "PanelRedIndicator";
            this.PanelRedIndicator.Size = new System.Drawing.Size(16, 16);
            this.PanelRedIndicator.TabIndex = 1;
            // 
            // LabelRedIndicator
            // 
            this.LabelRedIndicator.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LabelRedIndicator.AutoSize = true;
            this.LabelRedIndicator.Location = new System.Drawing.Point(32, 528);
            this.LabelRedIndicator.Name = "LabelRedIndicator";
            this.LabelRedIndicator.Size = new System.Drawing.Size(165, 15);
            this.LabelRedIndicator.TabIndex = 2;
            this.LabelRedIndicator.Text = "Wrong Processor Architecture";
            // 
            // LabelDarkGrayIndicator
            // 
            this.LabelDarkGrayIndicator.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LabelDarkGrayIndicator.AutoSize = true;
            this.LabelDarkGrayIndicator.Location = new System.Drawing.Point(32, 549);
            this.LabelDarkGrayIndicator.Name = "LabelDarkGrayIndicator";
            this.LabelDarkGrayIndicator.Size = new System.Drawing.Size(202, 15);
            this.LabelDarkGrayIndicator.TabIndex = 4;
            this.LabelDarkGrayIndicator.Text = "Invalid Architecture / System Process";
            // 
            // PanelDarkGrayIndicator
            // 
            this.PanelDarkGrayIndicator.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.PanelDarkGrayIndicator.BackColor = System.Drawing.Color.LightGray;
            this.PanelDarkGrayIndicator.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelDarkGrayIndicator.Location = new System.Drawing.Point(12, 549);
            this.PanelDarkGrayIndicator.Name = "PanelDarkGrayIndicator";
            this.PanelDarkGrayIndicator.Size = new System.Drawing.Size(16, 16);
            this.PanelDarkGrayIndicator.TabIndex = 3;
            // 
            // FilterTextBox
            // 
            this.FilterTextBox.Location = new System.Drawing.Point(12, 34);
            this.FilterTextBox.Name = "FilterTextBox";
            this.FilterTextBox.PlaceholderText = "Filter...";
            this.FilterTextBox.Size = new System.Drawing.Size(185, 23);
            this.FilterTextBox.TabIndex = 0;
            // 
            // ButtonSelectAndContinue
            // 
            this.ButtonSelectAndContinue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonSelectAndContinue.Enabled = false;
            this.ButtonSelectAndContinue.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.ButtonSelectAndContinue.Location = new System.Drawing.Point(570, 528);
            this.ButtonSelectAndContinue.Name = "ButtonSelectAndContinue";
            this.ButtonSelectAndContinue.Size = new System.Drawing.Size(90, 39);
            this.ButtonSelectAndContinue.TabIndex = 4;
            this.ButtonSelectAndContinue.Text = "Select";
            this.ButtonSelectAndContinue.UseVisualStyleBackColor = true;
            this.ButtonSelectAndContinue.Click += new System.EventHandler(this.ButtonSelectAndContinue_Click);
            // 
            // LabelGreenIndicator
            // 
            this.LabelGreenIndicator.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LabelGreenIndicator.AutoSize = true;
            this.LabelGreenIndicator.Location = new System.Drawing.Point(270, 529);
            this.LabelGreenIndicator.Name = "LabelGreenIndicator";
            this.LabelGreenIndicator.Size = new System.Drawing.Size(132, 15);
            this.LabelGreenIndicator.TabIndex = 9;
            this.LabelGreenIndicator.Text = "Pre-Configured Process";
            // 
            // PanelGreenIndicator
            // 
            this.PanelGreenIndicator.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.PanelGreenIndicator.BackColor = System.Drawing.Color.LimeGreen;
            this.PanelGreenIndicator.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelGreenIndicator.Location = new System.Drawing.Point(250, 529);
            this.PanelGreenIndicator.Name = "PanelGreenIndicator";
            this.PanelGreenIndicator.Size = new System.Drawing.Size(16, 16);
            this.PanelGreenIndicator.TabIndex = 8;
            // 
            // LabelWhiteIndicator
            // 
            this.LabelWhiteIndicator.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LabelWhiteIndicator.AutoSize = true;
            this.LabelWhiteIndicator.Location = new System.Drawing.Point(270, 550);
            this.LabelWhiteIndicator.Name = "LabelWhiteIndicator";
            this.LabelWhiteIndicator.Size = new System.Drawing.Size(151, 15);
            this.LabelWhiteIndicator.TabIndex = 11;
            this.LabelWhiteIndicator.Text = "Valid Unconfigured Process";
            // 
            // PanelWhiteIndicator
            // 
            this.PanelWhiteIndicator.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.PanelWhiteIndicator.BackColor = System.Drawing.Color.White;
            this.PanelWhiteIndicator.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelWhiteIndicator.Location = new System.Drawing.Point(250, 550);
            this.PanelWhiteIndicator.Name = "PanelWhiteIndicator";
            this.PanelWhiteIndicator.Size = new System.Drawing.Size(16, 16);
            this.PanelWhiteIndicator.TabIndex = 10;
            // 
            // ButtonClose
            // 
            this.ButtonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonClose.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ButtonClose.Location = new System.Drawing.Point(474, 528);
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.Size = new System.Drawing.Size(90, 39);
            this.ButtonClose.TabIndex = 3;
            this.ButtonClose.Text = "Back";
            this.ButtonClose.UseVisualStyleBackColor = true;
            this.ButtonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // HideInvalidCheckBox
            // 
            this.HideInvalidCheckBox.AutoSize = true;
            this.HideInvalidCheckBox.Location = new System.Drawing.Point(210, 36);
            this.HideInvalidCheckBox.Name = "HideInvalidCheckBox";
            this.HideInvalidCheckBox.Size = new System.Drawing.Size(89, 19);
            this.HideInvalidCheckBox.TabIndex = 1;
            this.HideInvalidCheckBox.Text = "Hide Invalid";
            this.HideInvalidCheckBox.UseVisualStyleBackColor = true;
            // 
            // HideWindowlessCheckBox
            // 
            this.HideWindowlessCheckBox.AutoSize = true;
            this.HideWindowlessCheckBox.Location = new System.Drawing.Point(305, 36);
            this.HideWindowlessCheckBox.Name = "HideWindowlessCheckBox";
            this.HideWindowlessCheckBox.Size = new System.Drawing.Size(117, 19);
            this.HideWindowlessCheckBox.TabIndex = 2;
            this.HideWindowlessCheckBox.Text = "Hide Windowless";
            this.HideWindowlessCheckBox.UseVisualStyleBackColor = true;
            // 
            // MenuStripMain
            // 
            this.MenuStripMain.BackColor = System.Drawing.Color.White;
            this.MenuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.MenuStripMain.Location = new System.Drawing.Point(0, 0);
            this.MenuStripMain.Name = "MainMenuStrip";
            this.MenuStripMain.Size = new System.Drawing.Size(672, 24);
            this.MenuStripMain.TabIndex = 12;
            this.MenuStripMain.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItem_Click);
            // 
            // ProcessSelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(672, 577);
            this.Controls.Add(this.HideWindowlessCheckBox);
            this.Controls.Add(this.HideInvalidCheckBox);
            this.Controls.Add(this.ButtonClose);
            this.Controls.Add(this.LabelWhiteIndicator);
            this.Controls.Add(this.PanelWhiteIndicator);
            this.Controls.Add(this.LabelGreenIndicator);
            this.Controls.Add(this.PanelGreenIndicator);
            this.Controls.Add(this.ButtonSelectAndContinue);
            this.Controls.Add(this.FilterTextBox);
            this.Controls.Add(this.LabelDarkGrayIndicator);
            this.Controls.Add(this.PanelDarkGrayIndicator);
            this.Controls.Add(this.LabelRedIndicator);
            this.Controls.Add(this.PanelRedIndicator);
            this.Controls.Add(this.ProcessDataGrid);
            this.Controls.Add(this.MenuStripMain);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.MenuStripMain;
            this.Name = "ProcessSelectForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RTTE ({0}) - Select Process";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ProcessSelectForm_FormClosing);
            this.Load += new System.EventHandler(this.ProcessSelectForm_Load);
            this.Shown += new System.EventHandler(this.ProcessSelectForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.ProcessDataGrid)).EndInit();
            this.MenuStripMain.ResumeLayout(false);
            this.MenuStripMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DoubleBufferedDataGridView ProcessDataGrid;
        private System.Windows.Forms.Panel PanelRedIndicator;
        private System.Windows.Forms.Label LabelRedIndicator;
        private System.Windows.Forms.Label LabelDarkGrayIndicator;
        private System.Windows.Forms.Panel PanelDarkGrayIndicator;
        private System.Windows.Forms.TextBox FilterTextBox;
        private System.Windows.Forms.Button ButtonSelectAndContinue;
        private System.Windows.Forms.Label LabelGreenIndicator;
        private System.Windows.Forms.Panel PanelGreenIndicator;
        private System.Windows.Forms.Label LabelWhiteIndicator;
        private System.Windows.Forms.Panel PanelWhiteIndicator;
        private System.Windows.Forms.Button ButtonClose;
        private System.Windows.Forms.CheckBox HideInvalidCheckBox;
        private System.Windows.Forms.CheckBox HideWindowlessCheckBox;
        private System.Windows.Forms.ToolTip HelpTooltip;
        private System.Windows.Forms.MenuStrip MenuStripMain;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
    }
}