using SS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadsheetGUI
{
    public partial class Spreadsheet : Form, ISpreadsheetWindow
    {
        public Spreadsheet()
        {
            InitializeComponent();
        }

        
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
        /// <param name="text"></param>
        public void SetCurrentCellText(string text)
        {
            CurrentCell_Text.Text = text;
        }
    }
}
