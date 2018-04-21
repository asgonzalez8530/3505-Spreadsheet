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
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.revertToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.howToUseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CellProperties = new System.Windows.Forms.GroupBox();
            this.connectButton = new System.Windows.Forms.Button();
            this.contents_button = new System.Windows.Forms.Button();
            this.CurrentCell_Text = new System.Windows.Forms.TextBox();
            this.Value_Text = new System.Windows.Forms.TextBox();
            this.Contents_Text = new System.Windows.Forms.TextBox();
            this.spreadsheetPanel1 = new SS.SpreadsheetPanel();
            this.pingTimer = new System.Windows.Forms.Timer(this.components);
            this.timeoutTimer = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1.SuspendLayout();
            this.CellProperties.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(8, 1, 0, 1);
            this.menuStrip1.Size = new System.Drawing.Size(1171, 24);
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
            this.closeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.Close_click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.revertToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 22);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.undoToolStripMenuItem.Text = "Undo";
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.UndoToolStripMenuItem_Click);
            // 
            // revertToolStripMenuItem
            // 
            this.revertToolStripMenuItem.Name = "revertToolStripMenuItem";
            this.revertToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.revertToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.revertToolStripMenuItem.Text = "Revert";
            this.revertToolStripMenuItem.Click += new System.EventHandler(this.RevertToolStripMenuItem_Click);
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
            this.CellProperties.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.CellProperties.Controls.Add(this.connectButton);
            this.CellProperties.Controls.Add(this.contents_button);
            this.CellProperties.Controls.Add(this.CurrentCell_Text);
            this.CellProperties.Controls.Add(this.Value_Text);
            this.CellProperties.Dock = System.Windows.Forms.DockStyle.Top;
            this.CellProperties.Location = new System.Drawing.Point(0, 24);
            this.CellProperties.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.CellProperties.Name = "CellProperties";
            this.CellProperties.Padding = new System.Windows.Forms.Padding(4);
            this.CellProperties.Size = new System.Drawing.Size(1171, 75);
            this.CellProperties.TabIndex = 4;
            this.CellProperties.TabStop = false;
            this.CellProperties.Text = "Current Selection";
            // 
            // connectButton
            // 
            this.connectButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.connectButton.Location = new System.Drawing.Point(744, 19);
            this.connectButton.Margin = new System.Windows.Forms.Padding(0);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(253, 37);
            this.connectButton.TabIndex = 7;
            this.connectButton.Text = "Connect to a Spreadsheet";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // contents_button
            // 
            this.contents_button.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.contents_button.Location = new System.Drawing.Point(620, 20);
            this.contents_button.Margin = new System.Windows.Forms.Padding(0);
            this.contents_button.Name = "contents_button";
            this.contents_button.Size = new System.Drawing.Size(117, 36);
            this.contents_button.TabIndex = 6;
            this.contents_button.Text = "Edit Cell";
            this.contents_button.UseVisualStyleBackColor = true;
            this.contents_button.Click += new System.EventHandler(this.contents_button_Click);
            // 
            // CurrentCell_Text
            // 
            this.CurrentCell_Text.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.CurrentCell_Text.Enabled = false;
            this.CurrentCell_Text.Location = new System.Drawing.Point(11, 33);
            this.CurrentCell_Text.Margin = new System.Windows.Forms.Padding(0);
            this.CurrentCell_Text.Name = "CurrentCell_Text";
            this.CurrentCell_Text.Size = new System.Drawing.Size(89, 22);
            this.CurrentCell_Text.TabIndex = 2;
            // 
            // Value_Text
            // 
            this.Value_Text.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Value_Text.Enabled = false;
            this.Value_Text.Location = new System.Drawing.Point(108, 33);
            this.Value_Text.Margin = new System.Windows.Forms.Padding(0);
            this.Value_Text.Name = "Value_Text";
            this.Value_Text.Size = new System.Drawing.Size(132, 22);
            this.Value_Text.TabIndex = 3;
            this.Value_Text.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Contents_Text
            // 
            this.Contents_Text.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Contents_Text.Location = new System.Drawing.Point(448, 237);
            this.Contents_Text.Margin = new System.Windows.Forms.Padding(4);
            this.Contents_Text.Name = "Contents_Text";
            this.Contents_Text.Size = new System.Drawing.Size(176, 22);
            this.Contents_Text.TabIndex = 5;
            // 
            // spreadsheetPanel1
            // 
            this.spreadsheetPanel1.AutoSize = true;
            this.spreadsheetPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.spreadsheetPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spreadsheetPanel1.Location = new System.Drawing.Point(0, 99);
            this.spreadsheetPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.spreadsheetPanel1.Name = "spreadsheetPanel1";
            this.spreadsheetPanel1.Size = new System.Drawing.Size(1171, 500);
            this.spreadsheetPanel1.TabIndex = 5;
            // 
            // pingTimer
            // 
            this.pingTimer.Interval = 10000;
            this.pingTimer.Tick += new System.EventHandler(this.pingTimer_Tick);
            // 
            // timeoutTimer
            // 
            this.timeoutTimer.Interval = 60000;
            this.timeoutTimer.Tick += new System.EventHandler(this.timeoutTimer_Tick);
            // 
            // Spreadsheet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1171, 599);
            this.Controls.Add(this.Contents_Text);
            this.Controls.Add(this.spreadsheetPanel1);
            this.Controls.Add(this.CellProperties);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(429, 310);
            this.Name = "Spreadsheet";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.CellProperties.ResumeLayout(false);
            this.CellProperties.PerformLayout();
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
        private SS.SpreadsheetPanel spreadsheetPanel1;
        private System.Windows.Forms.Button contents_button;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem revertToolStripMenuItem;
        private System.Windows.Forms.TextBox Contents_Text;
        private System.Windows.Forms.TextBox CurrentCell_Text;
        private System.Windows.Forms.TextBox Value_Text;
        private System.Windows.Forms.Timer pingTimer;
        private System.Windows.Forms.Timer timeoutTimer;
    }
}

