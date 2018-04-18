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
    public partial class Form3 : Form
    {
        public string NEWSHEET = "---CREATE NEW---";

        public Form3(string[] sheets)
        {
            InitializeComponent();

            comboBox.Items.Add(NEWSHEET);
            comboBox.Items.AddRange(sheets);
        }
    }
}
