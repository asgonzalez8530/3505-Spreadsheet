using SS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        event Action AboutText;

        event Action HowToUseText;


        SpreadsheetPanel GetSpreadsheetPanel();

        string CurrentCellText { set; }

        string ValueBoxText { set; }

        string ContentsBoxText { get; set; }

        string WindowText { get; set; }

        void CreateNew();


        void ShowErrorMessageBox(string message);

        bool ShowOkayCancelMessageBox(string message, string caption);

        void SetCellText(int row, int col, string v);

        void GetCellSelection(out int row, out int col);

        void CloseWindow();

        void SetDefaultAcceptButton();

        void SetFocusToContentBox();

        void AddFormClosingAction(Action FormClosingAction);

        void SetCellSelectionToDefault();
    }
}
