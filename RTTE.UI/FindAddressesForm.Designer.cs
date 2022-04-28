
namespace RTTE.UI {
    partial class FindAddressesForm {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FindAddressesForm));
            this.TextTextBox = new System.Windows.Forms.TextBox();
            this.LabelHelp = new System.Windows.Forms.Label();
            this.SearchButton = new System.Windows.Forms.Button();
            this.EncodingComboBox = new System.Windows.Forms.ComboBox();
            this.AddressHookDataGridView = new System.Windows.Forms.DataGridView();
            this.LabelResults = new System.Windows.Forms.Label();
            this.ButtonSelect = new System.Windows.Forms.Button();
            this.BackButton = new System.Windows.Forms.Button();
            this.ButtonClose = new System.Windows.Forms.Button();
            this.LabelFoundTextAddressHooks = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ProcessesDataGridView = new System.Windows.Forms.DataGridView();
            this.RefineButton = new System.Windows.Forms.Button();
            this.MenuStripMain = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.AddressHookDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProcessesDataGridView)).BeginInit();
            this.MenuStripMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // TextTextBox
            // 
            this.TextTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.TextTextBox.Location = new System.Drawing.Point(44, 105);
            this.TextTextBox.Name = "TextTextBox";
            this.TextTextBox.PlaceholderText = "Text in the application";
            this.TextTextBox.Size = new System.Drawing.Size(181, 23);
            this.TextTextBox.TabIndex = 0;
            // 
            // LabelHelp
            // 
            this.LabelHelp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LabelHelp.Location = new System.Drawing.Point(12, 34);
            this.LabelHelp.Name = "LabelHelp";
            this.LabelHelp.Size = new System.Drawing.Size(483, 60);
            this.LabelHelp.TabIndex = 1;
            this.LabelHelp.Text = resources.GetString("LabelHelp.Text");
            this.LabelHelp.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // SearchButton
            // 
            this.SearchButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.SearchButton.Enabled = false;
            this.SearchButton.Location = new System.Drawing.Point(300, 105);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(75, 23);
            this.SearchButton.TabIndex = 2;
            this.SearchButton.Text = "Search";
            this.SearchButton.UseVisualStyleBackColor = true;
            this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // EncodingComboBox
            // 
            this.EncodingComboBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.EncodingComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.EncodingComboBox.FormattingEnabled = true;
            this.EncodingComboBox.Location = new System.Drawing.Point(231, 105);
            this.EncodingComboBox.Name = "EncodingComboBox";
            this.EncodingComboBox.Size = new System.Drawing.Size(63, 23);
            this.EncodingComboBox.Sorted = true;
            this.EncodingComboBox.TabIndex = 3;
            // 
            // AddressHookDataGridView
            // 
            this.AddressHookDataGridView.AllowUserToAddRows = false;
            this.AddressHookDataGridView.AllowUserToDeleteRows = false;
            this.AddressHookDataGridView.AllowUserToResizeRows = false;
            this.AddressHookDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AddressHookDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.AddressHookDataGridView.Location = new System.Drawing.Point(11, 257);
            this.AddressHookDataGridView.Name = "AddressHookDataGridView";
            this.AddressHookDataGridView.ReadOnly = true;
            this.AddressHookDataGridView.RowHeadersVisible = false;
            this.AddressHookDataGridView.RowTemplate.Height = 25;
            this.AddressHookDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.AddressHookDataGridView.ShowEditingIcon = false;
            this.AddressHookDataGridView.Size = new System.Drawing.Size(483, 193);
            this.AddressHookDataGridView.TabIndex = 4;
            // 
            // LabelResults
            // 
            this.LabelResults.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LabelResults.Location = new System.Drawing.Point(29, 459);
            this.LabelResults.Name = "LabelResults";
            this.LabelResults.Size = new System.Drawing.Size(452, 25);
            this.LabelResults.TabIndex = 5;
            this.LabelResults.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ButtonSelect
            // 
            this.ButtonSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonSelect.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.ButtonSelect.Location = new System.Drawing.Point(420, 493);
            this.ButtonSelect.Name = "ButtonSelect";
            this.ButtonSelect.Size = new System.Drawing.Size(75, 25);
            this.ButtonSelect.TabIndex = 6;
            this.ButtonSelect.Text = "Select";
            this.ButtonSelect.UseVisualStyleBackColor = true;
            this.ButtonSelect.Click += new System.EventHandler(this.ButtonSelect_Click);
            // 
            // BackButton
            // 
            this.BackButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BackButton.Location = new System.Drawing.Point(339, 493);
            this.BackButton.Name = "BackButton";
            this.BackButton.Size = new System.Drawing.Size(75, 25);
            this.BackButton.TabIndex = 7;
            this.BackButton.Text = "Back";
            this.BackButton.UseVisualStyleBackColor = true;
            this.BackButton.Click += new System.EventHandler(this.BackButton_Click);
            // 
            // ButtonClose
            // 
            this.ButtonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonClose.Location = new System.Drawing.Point(258, 493);
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.Size = new System.Drawing.Size(75, 25);
            this.ButtonClose.TabIndex = 8;
            this.ButtonClose.Text = "Close";
            this.ButtonClose.UseVisualStyleBackColor = true;
            this.ButtonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // LabelFoundTextAddressHooks
            // 
            this.LabelFoundTextAddressHooks.AutoSize = true;
            this.LabelFoundTextAddressHooks.Location = new System.Drawing.Point(11, 239);
            this.LabelFoundTextAddressHooks.Name = "LabelFoundTextAddressHooks";
            this.LabelFoundTextAddressHooks.Size = new System.Drawing.Size(150, 15);
            this.LabelFoundTextAddressHooks.TabIndex = 9;
            this.LabelFoundTextAddressHooks.Text = "Found Text Address Hooks:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 143);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 15);
            this.label1.TabIndex = 10;
            this.label1.Text = "Processes:";
            // 
            // ProcessesDataGridView
            // 
            this.ProcessesDataGridView.AllowUserToAddRows = false;
            this.ProcessesDataGridView.AllowUserToDeleteRows = false;
            this.ProcessesDataGridView.AllowUserToResizeRows = false;
            this.ProcessesDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProcessesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ProcessesDataGridView.Location = new System.Drawing.Point(11, 161);
            this.ProcessesDataGridView.Name = "ProcessesDataGridView";
            this.ProcessesDataGridView.ReadOnly = true;
            this.ProcessesDataGridView.RowHeadersVisible = false;
            this.ProcessesDataGridView.RowTemplate.Height = 25;
            this.ProcessesDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ProcessesDataGridView.ShowEditingIcon = false;
            this.ProcessesDataGridView.Size = new System.Drawing.Size(483, 67);
            this.ProcessesDataGridView.TabIndex = 11;
            // 
            // RefineButton
            // 
            this.RefineButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.RefineButton.Enabled = false;
            this.RefineButton.Location = new System.Drawing.Point(381, 105);
            this.RefineButton.Name = "RefineButton";
            this.RefineButton.Size = new System.Drawing.Size(75, 23);
            this.RefineButton.TabIndex = 12;
            this.RefineButton.Text = "Refine";
            this.RefineButton.UseVisualStyleBackColor = true;
            this.RefineButton.Click += new System.EventHandler(this.RefineButton_Click);
            // 
            // MenuStripMain
            // 
            this.MenuStripMain.BackColor = System.Drawing.Color.White;
            this.MenuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.MenuStripMain.Location = new System.Drawing.Point(0, 0);
            this.MenuStripMain.Name = "MainMenuStrip";
            this.MenuStripMain.Size = new System.Drawing.Size(509, 24);
            this.MenuStripMain.TabIndex = 13;
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
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItem_Click);
            // 
            // FindAddressesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(509, 531);
            this.Controls.Add(this.RefineButton);
            this.Controls.Add(this.ProcessesDataGridView);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LabelFoundTextAddressHooks);
            this.Controls.Add(this.AddressHookDataGridView);
            this.Controls.Add(this.ButtonClose);
            this.Controls.Add(this.BackButton);
            this.Controls.Add(this.ButtonSelect);
            this.Controls.Add(this.LabelResults);
            this.Controls.Add(this.EncodingComboBox);
            this.Controls.Add(this.SearchButton);
            this.Controls.Add(this.LabelHelp);
            this.Controls.Add(this.TextTextBox);
            this.Controls.Add(this.MenuStripMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.MenuStripMain;
            this.Name = "FindAddressesForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RTTE ({0}) - Set Address Hooks";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FindAddressesForm_FormClosing);
            this.Load += new System.EventHandler(this.FindAddressesForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.AddressHookDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProcessesDataGridView)).EndInit();
            this.MenuStripMain.ResumeLayout(false);
            this.MenuStripMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TextTextBox;
        private System.Windows.Forms.Label LabelHelp;
        private System.Windows.Forms.Button SearchButton;
        private System.Windows.Forms.ComboBox EncodingComboBox;
        private System.Windows.Forms.DataGridView AddressHookDataGridView;
        private System.Windows.Forms.Label LabelResults;
        private System.Windows.Forms.Button ButtonSelect;
        private System.Windows.Forms.Button BackButton;
        private System.Windows.Forms.Button ButtonClose;
        private System.Windows.Forms.Label LabelFoundTextAddressHooks;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView ProcessesDataGridView;
        private System.Windows.Forms.Button RefineButton;
        private System.Windows.Forms.MenuStrip MenuStripMain;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
    }
}