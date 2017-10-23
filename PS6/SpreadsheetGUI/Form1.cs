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

            // register method with selectionChanged event
            spreadsheetPanel1.SelectionChanged += DisplayCurrentCellName;

            // set default spreadsheet location
            spreadsheetPanel1.SetSelection(0, 0);
            // make sure current cell is displaying properly
            DisplayCurrentCellName(spreadsheetPanel1);
        }

        /// <summary>
        /// Gets the currently selected cell's zero indexed row and column and sets
        /// the CurrentCell_Text to the normalized cell name. 
        /// </summary>
        /// <param name="ss"></param>
        private void DisplayCurrentCellName(SpreadsheetPanel ss)
        {
            int row, col;
            ss.GetSelection(out col, out row);
            CurrentCell_Text.Text = ConvertCellName(row, col);
            
        }

        /// <summary>
        /// Takes in the zero indexed row and col and converts it to the string
        /// representation of a cell name
        /// </summary>
        private string ConvertCellName(int row, int col)
        {
            int rowName = row + 1;
            char colName = (char)col;
            colName += 'A';

            return "" + colName + "" + rowName;
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
    }
}
