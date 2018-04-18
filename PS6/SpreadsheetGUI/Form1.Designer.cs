namespace SpreadsheetGUI
{
    partial class Spreadsheet
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.howToUseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CellProperties = new System.Windows.Forms.GroupBox();
            this.connectButton = new System.Windows.Forms.Button();
            this.contents_button = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.Contents_Text = new System.Windows.Forms.TextBox();
            this.CurrentCell_Label = new System.Windows.Forms.Label();
            this.Contents_Label = new System.Windows.Forms.Label();
            this.Value_Label = new System.Windows.Forms.Label();
            this.CurrentCell_Text = new System.Windows.Forms.TextBox();
            this.Value_Text = new System.Windows.Forms.TextBox();
            this.spreadsheetPanel1 = new SS.SpreadsheetPanel();
            this.menuStrip1.SuspendLayout();
            this.CellProperties.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(6, 1, 0, 1);
            this.menuStrip1.Size = new System.Drawing.Size(878, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 22);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.Close_click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.howToUseToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 22);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.AboutMenuItem_Click);
            // 
            // howToUseToolStripMenuItem
            // 
            this.howToUseToolStripMenuItem.Name = "howToUseToolStripMenuItem";
            this.howToUseToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.howToUseToolStripMenuItem.Text = "How to Use";
            this.howToUseToolStripMenuItem.Click += new System.EventHandler(this.HowToUseMenuItem_Click);
            // 
            // CellProperties
            // 
            this.CellProperties.AutoSize = true;
            this.CellProperties.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CellProperties.Controls.Add(this.connectButton);
            this.CellProperties.Controls.Add(this.contents_button);
            this.CellProperties.Controls.Add(this.tableLayoutPanel2);
            this.CellProperties.Dock = System.Windows.Forms.DockStyle.Top;
            this.CellProperties.Location = new System.Drawing.Point(0, 24);
            this.CellProperties.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.CellProperties.Name = "CellProperties";
            this.CellProperties.Size = new System.Drawing.Size(878, 123);
            this.CellProperties.TabIndex = 4;
            this.CellProperties.TabStop = false;
            this.CellProperties.Text = "Cell Properties";
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(688, 23);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(178, 61);
            this.connectButton.TabIndex = 7;
            this.connectButton.Text = "Connect to Spreadsheet Server";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // contents_button
            // 
            this.contents_button.Location = new System.Drawing.Point(481, 60);
            this.contents_button.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.contents_button.Name = "contents_button";
            this.contents_button.Size = new System.Drawing.Size(88, 29);
            this.contents_button.TabIndex = 6;
            this.contents_button.Text = "Enter Contents";
            this.contents_button.UseVisualStyleBackColor = true;
            this.contents_button.Click += new System.EventHandler(this.contents_button_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.Contents_Text, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.CurrentCell_Label, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.Contents_Label, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.Value_Label, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.CurrentCell_Text, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.Value_Text, 3, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(473, 90);
            this.tableLayoutPanel2.TabIndex = 5;
            // 
            // Contents_Text
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.Contents_Text, 3);
            this.Contents_Text.Location = new System.Drawing.Point(66, 48);
            this.Contents_Text.Name = "Contents_Text";
            this.Contents_Text.Size = new System.Drawing.Size(399, 20);
            this.Contents_Text.TabIndex = 5;
            // 
            // CurrentCell_Label
            // 
            this.CurrentCell_Label.AutoSize = true;
            this.CurrentCell_Label.Dock = System.Windows.Forms.DockStyle.Right;
            this.CurrentCell_Label.Location = new System.Drawing.Point(1, 0);
            this.CurrentCell_Label.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.CurrentCell_Label.Name = "CurrentCell_Label";
            this.CurrentCell_Label.Size = new System.Drawing.Size(61, 45);
            this.CurrentCell_Label.TabIndex = 1;
            this.CurrentCell_Label.Text = "Current Cell";
            this.CurrentCell_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Contents_Label
            // 
            this.Contents_Label.AutoSize = true;
            this.Contents_Label.Dock = System.Windows.Forms.DockStyle.Right;
            this.Contents_Label.Location = new System.Drawing.Point(11, 45);
            this.Contents_Label.Name = "Contents_Label";
            this.Contents_Label.Size = new System.Drawing.Size(49, 45);
            this.Contents_Label.TabIndex = 4;
            this.Contents_Label.Text = "Contents";
            // 
            // Value_Label
            // 
            this.Value_Label.AutoSize = true;
            this.Value_Label.Dock = System.Windows.Forms.DockStyle.Right;
            this.Value_Label.Location = new System.Drawing.Point(137, 0);
            this.Value_Label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Value_Label.Name = "Value_Label";
            this.Value_Label.Size = new System.Drawing.Size(34, 45);
            this.Value_Label.TabIndex = 2;
            this.Value_Label.Text = "Value";
            this.Value_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CurrentCell_Text
            // 
            this.CurrentCell_Text.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.CurrentCell_Text.Enabled = false;
            this.CurrentCell_Text.Location = new System.Drawing.Point(65, 12);
            this.CurrentCell_Text.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.CurrentCell_Text.Name = "CurrentCell_Text";
            this.CurrentCell_Text.Size = new System.Drawing.Size(68, 20);
            this.CurrentCell_Text.TabIndex = 2;
            // 
            // Value_Text
            // 
            this.Value_Text.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Value_Text.Enabled = false;
            this.Value_Text.Location = new System.Drawing.Point(176, 12);
            this.Value_Text.Name = "Value_Text";
            this.Value_Text.Size = new System.Drawing.Size(100, 20);
            this.Value_Text.TabIndex = 3;
            this.Value_Text.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // spreadsheetPanel1
            // 
            this.spreadsheetPanel1.AutoSize = true;
            this.spreadsheetPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.spreadsheetPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spreadsheetPanel1.Location = new System.Drawing.Point(0, 147);
            this.spreadsheetPanel1.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.spreadsheetPanel1.Name = "spreadsheetPanel1";
            this.spreadsheetPanel1.Size = new System.Drawing.Size(878, 340);
            this.spreadsheetPanel1.TabIndex = 5;
            // 
            // Spreadsheet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(878, 487);
            this.Controls.Add(this.spreadsheetPanel1);
            this.Controls.Add(this.CellProperties);
            this.Controls.Add(this.menuStrip1);
            this.MinimumSize = new System.Drawing.Size(326, 259);
            this.Name = "Spreadsheet";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.CellProperties.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem howToUseToolStripMenuItem;
        private System.Windows.Forms.GroupBox CellProperties;
        private System.Windows.Forms.Label Contents_Label;
        private System.Windows.Forms.TextBox Value_Text;
        private System.Windows.Forms.TextBox Contents_Text;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label CurrentCell_Label;
        private System.Windows.Forms.TextBox CurrentCell_Text;
        private System.Windows.Forms.Label Value_Label;
        private SS.SpreadsheetPanel spreadsheetPanel1;
        private System.Windows.Forms.Button contents_button;
        private System.Windows.Forms.Button connectButton;
    }
}

