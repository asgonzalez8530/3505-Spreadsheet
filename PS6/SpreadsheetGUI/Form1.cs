using SS;
using System;
using System.Windows.Forms;

/// <summary>
/// This program is a GUI of the client spreadsheet for a multi-client Spreadsheet Server.
/// 
/// 3505 version of ClientGUI by Rebekah Peterson, Jacqulyn Machardy, Anastasia Gonzalez, Michael Raleigh
/// 3500 version of SpreadsheetGUI by Anastasia Gonzalez, Aaron Bellis
/// </summary>

namespace SpreadsheetGUI
{
    public partial class Spreadsheet : Form, ISpreadsheetWindow
    {
        // Constructor
        public Spreadsheet()
        {
            InitializeComponent();
        }

        // Action listeners
        public event Action EnterContentsAction;
        public event Action SaveFileAction;
        public event Action OpenFileAction;
        public event Action AboutText;
        public event Action HowToUseText;

        // Added for 3505
        public event Action Startup;
        public event Action Undo;
        public event Action Revert;
        public event Action Ping;
        public event Action Timeout;
        public event Action ArrowSelectionChanged;

        /// <summary>
        /// Gets the spreadsheet panel component in this window
        /// </summary>
        public SpreadsheetPanel GetSpreadsheetPanel()
        {
            return spreadsheetPanel1;
        }

        /// <summary>
        /// Sets the text field of the CurrentCell_Text component
        /// </summary>
        public string CurrentCellText
        {
            get { return CurrentCell_Text.Text; }
            set { CurrentCell_Text.Text = value; }
        }

        /// <summary>
        /// Sets the Value_Text.Text
        /// </summary>
        public string ValueBoxText { set { Value_Text.Text = value; } }

        /// <summary>
        /// Sets the Contents_Text.Text
        /// </summary>
        public string ContentsBoxText { get => Contents_Text.Text; set { Contents_Text.Text = value; } }

        /// <summary>
        /// Sets the text of this window
        /// </summary>
        public string WindowText
        {
            get => Text;
            set
            {
                MethodInvoker m = new MethodInvoker(() => { Text = value; });
                Invoke(m);
            }

        }

        /// <summary>
        /// Method evoked when the File -> new is clicked
        /// </summary>
        private void FileMenuNew_Click(object sender, EventArgs e)
        {
            //// fire off event if listeners are registered.
            //if (NewSheetAction != null)
            //{
            //    NewSheetAction();
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a new window containing an empty spreadsheet.
        /// </summary>
        public void CreateNew()
        {
            DemoApplicationContext.getAppContext().RunForm(new Spreadsheet());
        }

        /// <summary>
        /// Shows an error message box with the coresponding message
        /// </summary>
        public void ShowErrorMessageBox(string message)
        {
            MessageBox.Show(message, "Connection state");
        }

        /// <summary>
        /// Sets the cell in the SpreadsheetPanel located at row, col to the string text
        /// </summary>
        public void SetCellText(int row, int col, string text)
        {
            spreadsheetPanel1.SetValue(col, row, text);
        }

        /// <summary>
        /// Gets the currently selected cell location
        /// </summary>
        public void GetCellSelection(out int row, out int col)
        {
            spreadsheetPanel1.GetSelection(out col, out row);
        }

        /// <summary>
        /// Notify controller when contents_button has been clicked
        /// </summary>
        private void contents_button_Click(object sender, EventArgs e)
        {
            EnterContentsAction();
        }

        /// <summary>
        /// Allows controller to close this window
        /// </summary>
        public void CloseWindow()
        {
            Close();
        }

        /// <summary>
        /// Sets the default accept button as contents_button
        /// </summary>
        public void SetDefaultAcceptButton()
        {
            AcceptButton = contents_button;
        }


        /// <summary>
        /// Fired when the Close menu item is clicked
        /// </summary>
        private void Close_click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Sets focus to the contents_text box
        /// </summary>
        public void SetFocusToContentBox()
        {
            Contents_Text.Focus();
        }

        /// <summary>
        /// Fired with the Save menu item is clicked
        /// </summary>
        private void SaveFile_Click(object sender, EventArgs e)
        {
            SaveFileAction();
        }

        /// <summary>
        /// Shows a message box which can be canceled with a coresponding message and caption
        /// </summary>
        public bool ShowOkayCancelMessageBox(string message, string caption)
        {
            string messageBoxText = message;
            string cap = caption;
            MessageBoxButtons button = MessageBoxButtons.OKCancel;

            // Display message box
            DialogResult result = MessageBox.Show(messageBoxText, caption, button);

            // Process message box results
            switch (result)
            {
                case DialogResult.OK:
                    return true;
                default:
                    return false;
            }
        }



        /// <summary>
        /// Fired when The Open menu item is clicked
        /// </summary>
        private void OpenFile_Click(object sender, EventArgs e)
        {
            OpenFileAction();
        }

        /// <summary>
        /// Allows controller to add an action to the FormClosing event
        /// </summary>
        public void AddFormClosingAction(Action FormClosingAction)
        {
            FormClosing += (x, y) => FormClosingAction();
        }

        /// <summary>
        /// sets the default cell as selected in the spreadsheet panel
        /// </summary>
        public void SetCellSelectionToDefault()
        {
            spreadsheetPanel1.SetSelection(0,0);
        }

        public void UpdateEditBoxLocation(int x, int y, int width, int height)
        {
            Contents_Text.Location = new System.Drawing.Point(x, spreadsheetPanel1.Location.Y + y);
            Contents_Text.Width = width;
            Contents_Text.Height = height;
        }

        public void StartPinging()
        {
            pingTimer.Start();
            timeoutTimer.Start();
        }

        public void StopPinging()
        {
            pingTimer.Stop();
            timeoutTimer.Stop();
        }

        public void ResetTimeout()
        {
            timeoutTimer.Stop();
            timeoutTimer.Start();
        }

        /// <summary>
        /// Fired when about menu item is clicked
        /// </summary>
        private void AboutMenuItem_Click(object sender, EventArgs e)
        {
            AboutText();
        }

        /// <summary>
        /// Fired when the how to use menu item is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HowToUseMenuItem_Click(object sender, EventArgs e)
        {
            HowToUseText();
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            Startup();
        }

        private void RevertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Revert();
        }


        private void UndoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Undo();
        }

        private void PingTimer_Tick(object sender, EventArgs e)
        {
            Ping();
            pingTimer.Start();
        }

        private void TimeoutTimer_Tick(object sender, EventArgs e)
        {
            Timeout();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Right || keyData == Keys.Left || keyData == Keys.Up || keyData == Keys.Down)
            {
                int MAX_ROW = 98;
                int MAX_COL = 25;
                spreadsheetPanel1.GetSelection(out int col, out int row);

                if (keyData == Keys.Right && col != MAX_COL)
                    col++;
                if (keyData == Keys.Left && col != 0)
                     col--;
                if (keyData == Keys.Up && row != 0)
                    row--;
                if (keyData == Keys.Down && row != MAX_ROW)
                    row++;

                spreadsheetPanel1.SetSelection(col, row);

                ArrowSelectionChanged();
                return true;
            }
            else
            {
                return base.ProcessCmdKey(ref msg, keyData);
            }
        }
    }
}
