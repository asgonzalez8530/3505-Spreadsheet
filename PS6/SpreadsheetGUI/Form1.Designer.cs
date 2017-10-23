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
            this.menuStrip1.Size = new System.Drawing.Size(807, 24);
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
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            this.fileToolStripMenuItem.Click += new System.EventHandler(this.fIelToolStripMenuItem_Click);
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.newToolStripMenuItem.Text = "New";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.openToolStripMenuItem.Text = "Open";
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.closeToolStripMenuItem.Text = "Close";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.howToUseToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // howToUseToolStripMenuItem
            // 
            this.howToUseToolStripMenuItem.Name = "howToUseToolStripMenuItem";
            this.howToUseToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.howToUseToolStripMenuItem.Text = "How to Use";
            // 
            // CellProperties
            // 
            this.CellProperties.Controls.Add(this.tableLayoutPanel2);
            this.CellProperties.Dock = System.Windows.Forms.DockStyle.Top;
            this.CellProperties.Location = new System.Drawing.Point(0, 24);
            this.CellProperties.Margin = new System.Windows.Forms.Padding(2);
            this.CellProperties.Name = "CellProperties";
            this.CellProperties.Size = new System.Drawing.Size(807, 109);
            this.CellProperties.TabIndex = 4;
            this.CellProperties.TabStop = false;
            this.CellProperties.Text = "Cell Properties";
            // 
            // tableLayoutPanel2
            // 
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
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(801, 90);
            this.tableLayoutPanel2.TabIndex = 5;
            // 
            // Contents_Text
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.Contents_Text, 3);
            this.Contents_Text.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Contents_Text.Location = new System.Drawing.Point(68, 48);
            this.Contents_Text.Name = "Contents_Text";
            this.Contents_Text.Size = new System.Drawing.Size(1250, 20);
            this.Contents_Text.TabIndex = 5;
            this.Contents_Text.TextChanged += new System.EventHandler(this.Contents_Text_TextChanged);
            // 
            // CurrentCell_Label
            // 
            this.CurrentCell_Label.AutoSize = true;
            this.CurrentCell_Label.Dock = System.Windows.Forms.DockStyle.Right;
            this.CurrentCell_Label.Location = new System.Drawing.Point(2, 0);
            this.CurrentCell_Label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.CurrentCell_Label.Name = "CurrentCell_Label";
            this.CurrentCell_Label.Size = new System.Drawing.Size(61, 45);
            this.CurrentCell_Label.TabIndex = 1;
            this.CurrentCell_Label.Text = "Current Cell";
            // 
            // Contents_Label
            // 
            this.Contents_Label.AutoSize = true;
            this.Contents_Label.Dock = System.Windows.Forms.DockStyle.Right;
            this.Contents_Label.Location = new System.Drawing.Point(13, 45);
            this.Contents_Label.Name = "Contents_Label";
            this.Contents_Label.Size = new System.Drawing.Size(49, 45);
            this.Contents_Label.TabIndex = 4;
            this.Contents_Label.Text = "Contents";
            this.Contents_Label.Click += new System.EventHandler(this.label1_Click);
            // 
            // CurrentCell_Text
            // 
            this.CurrentCell_Text.Enabled = false;
            this.CurrentCell_Text.Location = new System.Drawing.Point(67, 2);
            this.CurrentCell_Text.Margin = new System.Windows.Forms.Padding(2);
            this.CurrentCell_Text.Name = "CurrentCell_Text";
            this.CurrentCell_Text.Size = new System.Drawing.Size(68, 20);
            this.CurrentCell_Text.TabIndex = 2;
            // 
            // Value_Text
            // 
            this.Value_Text.Enabled = false;
            this.Value_Text.Location = new System.Drawing.Point(178, 3);
            this.Value_Text.Name = "Value_Text";
            this.Value_Text.Size = new System.Drawing.Size(100, 20);
            this.Value_Text.TabIndex = 3;
            // 
            // Value_Label
            // 
            this.Value_Label.AutoSize = true;
            this.Value_Label.Dock = System.Windows.Forms.DockStyle.Right;
            this.Value_Label.Location = new System.Drawing.Point(139, 0);
            this.Value_Label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Value_Label.Name = "Value_Label";
            this.Value_Label.Size = new System.Drawing.Size(34, 45);
            this.Value_Label.TabIndex = 2;
            this.Value_Label.Text = "Value";
            // 
            // spreadsheetPanel1
            // 
            this.spreadsheetPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spreadsheetPanel1.Location = new System.Drawing.Point(0, 133);
            this.spreadsheetPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.spreadsheetPanel1.Name = "spreadsheetPanel1";
            this.spreadsheetPanel1.Size = new System.Drawing.Size(807, 346);
            this.spreadsheetPanel1.TabIndex = 5;
            // 
            // Spreadsheet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(807, 479);
            this.Controls.Add(this.spreadsheetPanel1);
            this.Controls.Add(this.CellProperties);
            this.Controls.Add(this.menuStrip1);
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
    }
}

