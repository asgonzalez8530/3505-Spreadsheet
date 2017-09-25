// Written by Aaron Bellis u0981638 for CS 3500 2017 Fall Semester.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadsheetUtilities;

namespace SS
{
    public class Spreadsheet : AbstractSpreadsheet
    {
        protected Spreadsheet()
        {
            
        }

        public override object GetCellContents(string name)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            throw new NotImplementedException();
        }

        public override ISet<string> SetCellContents(string name, double number)
        {
            throw new NotImplementedException();
        }

        public override ISet<string> SetCellContents(string name, string text)
        {
            throw new NotImplementedException();
        }

        public override ISet<string> SetCellContents(string name, Formula formula)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            throw new NotImplementedException();
        }
    }
}
