using SS;
using System;
using System.Windows.Forms;

namespace SpreadsheetGUI
{
    public partial class Spreadsheet : Form, ISpreadsheetWindow
    {

        public Spreadsheet()
        {
            InitializeComponent();

            
            //TODO: make a spreadsheet that has a data path
        }

        public event Action NewSheetAction;
        public event Action EnterContentsAction;
        public event Action SaveFileAction;
        public event Action OpenFileAction;
        

        private void fIelToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void CurrentCell_Label_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Contents_Text_TextChanged(object sender, EventArgs e)
        {

        }

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
        public string WindowText { get => Text; set => Text = value; }

        /// <summary>
        /// Method evoked when the File -> new is clicked
        /// </summary>
        private void FileMenuNew_Click(object sender, EventArgs e)
        {
            // fire off event if listeners are registered.
            if (NewSheetAction != null)
            {
                NewSheetAction();
            }

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
        /// <param name="message"></param>
        public void ShowErrorMessageBox(string message)
        {
            MessageBox.Show(message);
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
            this.AcceptButton = contents_button;
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

    }
}
