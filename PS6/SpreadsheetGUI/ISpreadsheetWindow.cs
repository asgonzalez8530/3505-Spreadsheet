using SS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SpreadsheetGUI
{
    /// <summary>
    /// Controllable interface for the Spreadsheet GUI
    /// </summary>
    public interface ISpreadsheetWindow
    {
        event Action NewSheetAction;

        event Action EnterContentsAction;

        event Action SaveFileAction;

        event Action OpenFileAction;

        

        SpreadsheetPanel GetSpreadsheetPanel();

        string CurrentCellText { set; }

        string ValueBoxText { set; }

        string ContentsBoxText { get; set; }

        string WindowText { get; set; }

        void CreateNew();

        void CreateNew(string fileName);

        void ShowErrorMessageBox(string message);

        bool ShowOkayCancelMessageBox(string message, string caption);

        void SetCellText(int row, int col, string v);

        void GetCellSelection(out int row, out int col);

        void CloseWindow();

        void SetDefaultAcceptButton();

        void SetFocusToContentBox();

        void AddFormClosingAction(Action FormClosingAction);

        
    }
}
