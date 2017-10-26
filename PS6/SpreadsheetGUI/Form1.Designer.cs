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
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.Contents_Text = new System.Windows.Forms.TextBox();
            this.CurrentCell_Label = new System.Windows.Forms.Label();
            this.Contents_Label = new System.Windows.Forms.Label();
            this.CurrentCell_Text = new System.Windows.Forms.TextBox();
            this.Value_Text = new System.Windows.Forms.TextBox();
            this.Value_Label = new System.Windows.Forms.Label();
            this.spreadsheetPanel1 = new SS.SpreadsheetPanel();
            this.contents_button = new System.Windows.Forms.Button();
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
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1171, 28);
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
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
            this.fileToolStripMenuItem.Text = "File";
            this.fileToolStripMenuItem.Click += new System.EventHandler(this.fIelToolStripMenuItem_Click);
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(120, 26);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.FileMenuNew_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(120, 26);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(120, 26);
            this.openToolStripMenuItem.Text = "Open";
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.ToolStripClose);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.howToUseToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(161, 26);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // howToUseToolStripMenuItem
            // 
            this.howToUseToolStripMenuItem.Name = "howToUseToolStripMenuItem";
            this.howToUseToolStripMenuItem.Size = new System.Drawing.Size(161, 26);
            this.howToUseToolStripMenuItem.Text = "How to Use";
            // 
            // CellProperties
            // 
            this.CellProperties.AutoSize = true;
            this.CellProperties.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CellProperties.Controls.Add(this.contents_button);
            this.CellProperties.Controls.Add(this.tableLayoutPanel2);
            this.CellProperties.Dock = System.Windows.Forms.DockStyle.Top;
            this.CellProperties.Location = new System.Drawing.Point(0, 28);
            this.CellProperties.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.CellProperties.Name = "CellProperties";
            this.CellProperties.Padding = new System.Windows.Forms.Padding(4);
            this.CellProperties.Size = new System.Drawing.Size(1171, 151);
            this.CellProperties.TabIndex = 4;
            this.CellProperties.TabStop = false;
            this.CellProperties.Text = "Cell Properties";
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
            this.tableLayoutPanel2.Controls.Add(this.CurrentCell_Text, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.Value_Text, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.Value_Label, 2, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(4, 19);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(631, 111);
            this.tableLayoutPanel2.TabIndex = 5;
            // 
            // Contents_Text
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.Contents_Text, 3);
            this.Contents_Text.Location = new System.Drawing.Point(92, 59);
            this.Contents_Text.Margin = new System.Windows.Forms.Padding(4);
            this.Contents_Text.Name = "Contents_Text";
            this.Contents_Text.Size = new System.Drawing.Size(530, 22);
            this.Contents_Text.TabIndex = 5;
            // 
            // CurrentCell_Label
            // 
            this.CurrentCell_Label.AutoSize = true;
            this.CurrentCell_Label.Dock = System.Windows.Forms.DockStyle.Right;
            this.CurrentCell_Label.Location = new System.Drawing.Point(3, 0);
            this.CurrentCell_Label.Name = "CurrentCell_Label";
            this.CurrentCell_Label.Size = new System.Drawing.Size(82, 55);
            this.CurrentCell_Label.TabIndex = 1;
            this.CurrentCell_Label.Text = "Current Cell";
            // 
            // Contents_Label
            // 
            this.Contents_Label.AutoSize = true;
            this.Contents_Label.Dock = System.Windows.Forms.DockStyle.Right;
            this.Contents_Label.Location = new System.Drawing.Point(20, 55);
            this.Contents_Label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Contents_Label.Name = "Contents_Label";
            this.Contents_Label.Size = new System.Drawing.Size(64, 56);
            this.Contents_Label.TabIndex = 4;
            this.Contents_Label.Text = "Contents";
            this.Contents_Label.Click += new System.EventHandler(this.label1_Click);
            // 
            // CurrentCell_Text
            // 
            this.CurrentCell_Text.Enabled = false;
            this.CurrentCell_Text.Location = new System.Drawing.Point(91, 2);
            this.CurrentCell_Text.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.CurrentCell_Text.Name = "CurrentCell_Text";
            this.CurrentCell_Text.Size = new System.Drawing.Size(89, 22);
            this.CurrentCell_Text.TabIndex = 2;
            // 
            // Value_Text
            // 
            this.Value_Text.Enabled = false;
            this.Value_Text.Location = new System.Drawing.Point(237, 4);
            this.Value_Text.Margin = new System.Windows.Forms.Padding(4);
            this.Value_Text.Name = "Value_Text";
            this.Value_Text.Size = new System.Drawing.Size(132, 22);
            this.Value_Text.TabIndex = 3;
            // 
            // Value_Label
            // 
            this.Value_Label.AutoSize = true;
            this.Value_Label.Dock = System.Windows.Forms.DockStyle.Right;
            this.Value_Label.Location = new System.Drawing.Point(186, 0);
            this.Value_Label.Name = "Value_Label";
            this.Value_Label.Size = new System.Drawing.Size(44, 55);
            this.Value_Label.TabIndex = 2;
            this.Value_Label.Text = "Value";
            // 
            // spreadsheetPanel1
            // 
            this.spreadsheetPanel1.AutoSize = true;
            this.spreadsheetPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.spreadsheetPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spreadsheetPanel1.Location = new System.Drawing.Point(0, 179);
            this.spreadsheetPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.spreadsheetPanel1.Name = "spreadsheetPanel1";
            this.spreadsheetPanel1.Size = new System.Drawing.Size(1171, 463);
            this.spreadsheetPanel1.TabIndex = 5;
            // 
            // contents_button
            // 
            this.contents_button.Location = new System.Drawing.Point(641, 74);
            this.contents_button.Name = "contents_button";
            this.contents_button.Size = new System.Drawing.Size(117, 35);
            this.contents_button.TabIndex = 6;
            this.contents_button.Text = "Enter Contents";
            this.contents_button.UseVisualStyleBackColor = true;
            this.contents_button.Click += new System.EventHandler(this.contents_button_Click);
            // 
            // Spreadsheet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1171, 642);
            this.Controls.Add(this.spreadsheetPanel1);
            this.Controls.Add(this.CellProperties);
            this.Controls.Add(this.menuStrip1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(430, 315);
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

