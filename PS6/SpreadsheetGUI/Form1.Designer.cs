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
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.howToUseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CellProperties = new System.Windows.Forms.GroupBox();
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
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(9, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1317, 33);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.openToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(140, 29);
            this.fileToolStripMenuItem.Text = "File";
            this.fileToolStripMenuItem.Click += new System.EventHandler(this.fileToolStripMenuItem_Click);
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(210, 30);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.FileMenuNew_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(210, 30);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.SaveFile_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(210, 30);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.OpenFile_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(210, 30);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.Close_click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.howToUseToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(61, 29);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(189, 30);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // howToUseToolStripMenuItem
            // 
            this.howToUseToolStripMenuItem.Name = "howToUseToolStripMenuItem";
            this.howToUseToolStripMenuItem.Size = new System.Drawing.Size(189, 30);
            this.howToUseToolStripMenuItem.Text = "How to Use";
            // 
            // CellProperties
            // 
            this.CellProperties.AutoSize = true;
            this.CellProperties.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CellProperties.Controls.Add(this.contents_button);
            this.CellProperties.Controls.Add(this.tableLayoutPanel2);
            this.CellProperties.Dock = System.Windows.Forms.DockStyle.Top;
            this.CellProperties.Location = new System.Drawing.Point(0, 33);
            this.CellProperties.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.CellProperties.Name = "CellProperties";
            this.CellProperties.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.CellProperties.Size = new System.Drawing.Size(1317, 189);
            this.CellProperties.TabIndex = 4;
            this.CellProperties.TabStop = false;
            this.CellProperties.Text = "Cell Properties";
            // 
            // contents_button
            // 
            this.contents_button.Location = new System.Drawing.Point(721, 92);
            this.contents_button.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.contents_button.Name = "contents_button";
            this.contents_button.Size = new System.Drawing.Size(132, 44);
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
            this.tableLayoutPanel2.Location = new System.Drawing.Point(4, 24);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(710, 139);
            this.tableLayoutPanel2.TabIndex = 5;
            // 
            // Contents_Text
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.Contents_Text, 3);
            this.Contents_Text.Location = new System.Drawing.Point(102, 74);
            this.Contents_Text.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Contents_Text.Name = "Contents_Text";
            this.Contents_Text.Size = new System.Drawing.Size(596, 26);
            this.Contents_Text.TabIndex = 5;
            // 
            // CurrentCell_Label
            // 
            this.CurrentCell_Label.AutoSize = true;
            this.CurrentCell_Label.Dock = System.Windows.Forms.DockStyle.Right;
            this.CurrentCell_Label.Location = new System.Drawing.Point(3, 0);
            this.CurrentCell_Label.Name = "CurrentCell_Label";
            this.CurrentCell_Label.Size = new System.Drawing.Size(92, 69);
            this.CurrentCell_Label.TabIndex = 1;
            this.CurrentCell_Label.Text = "Current Cell";
            this.CurrentCell_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Contents_Label
            // 
            this.Contents_Label.AutoSize = true;
            this.Contents_Label.Dock = System.Windows.Forms.DockStyle.Right;
            this.Contents_Label.Location = new System.Drawing.Point(20, 69);
            this.Contents_Label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Contents_Label.Name = "Contents_Label";
            this.Contents_Label.Size = new System.Drawing.Size(74, 70);
            this.Contents_Label.TabIndex = 4;
            this.Contents_Label.Text = "Contents";
            this.Contents_Label.Click += new System.EventHandler(this.label1_Click);
            // 
            // Value_Label
            // 
            this.Value_Label.AutoSize = true;
            this.Value_Label.Dock = System.Windows.Forms.DockStyle.Right;
            this.Value_Label.Location = new System.Drawing.Point(207, 0);
            this.Value_Label.Name = "Value_Label";
            this.Value_Label.Size = new System.Drawing.Size(50, 69);
            this.Value_Label.TabIndex = 2;
            this.Value_Label.Text = "Value";
            this.Value_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CurrentCell_Text
            // 
            this.CurrentCell_Text.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.CurrentCell_Text.Enabled = false;
            this.CurrentCell_Text.Location = new System.Drawing.Point(101, 21);
            this.CurrentCell_Text.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.CurrentCell_Text.Name = "CurrentCell_Text";
            this.CurrentCell_Text.Size = new System.Drawing.Size(100, 26);
            this.CurrentCell_Text.TabIndex = 2;
            // 
            // Value_Text
            // 
            this.Value_Text.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Value_Text.Enabled = false;
            this.Value_Text.Location = new System.Drawing.Point(264, 21);
            this.Value_Text.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Value_Text.Name = "Value_Text";
            this.Value_Text.Size = new System.Drawing.Size(148, 26);
            this.Value_Text.TabIndex = 3;
            this.Value_Text.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // spreadsheetPanel1
            // 
            this.spreadsheetPanel1.AutoSize = true;
            this.spreadsheetPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.spreadsheetPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spreadsheetPanel1.Location = new System.Drawing.Point(0, 222);
            this.spreadsheetPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.spreadsheetPanel1.Name = "spreadsheetPanel1";
            this.spreadsheetPanel1.Size = new System.Drawing.Size(1317, 580);
            this.spreadsheetPanel1.TabIndex = 5;
            // 
            // Spreadsheet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1317, 802);
            this.Controls.Add(this.spreadsheetPanel1);
            this.Controls.Add(this.CellProperties);
            this.Controls.Add(this.menuStrip1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(481, 377);
            this.Name = "Spreadsheet";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
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
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
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
    }
}

