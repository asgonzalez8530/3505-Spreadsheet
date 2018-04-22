using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

/// <summary>
/// This program is a GUI of the client spreadsheet for a multi-client Spreadsheet Server.
/// 
/// 3505 version of ClientGUI by Rebekah Peterson, Jacqulyn Machardy, Anastasia Gonzalez, Michael Raleigh
/// 3500 version of SpreadsheetGUI by Anastasia Gonzalez, Aaron Bellis
/// </summary>

namespace SpreadsheetGUI
{
    /// <summary>
    /// Keeps track of how many top-level forms are running
    /// </summary>
    class DemoApplicationContext : ApplicationContext
    {
        // Number of open forms
        private int formCount = 0;

        // Singleton ApplicationContext
        private static DemoApplicationContext appContext;

        /// <summary>
        /// Private constructor for singleton pattern
        /// </summary>
        private DemoApplicationContext()
        {

        }

        /// <summary>
        /// Returns the one DemoApplicationContext.
        /// </summary>
        public static DemoApplicationContext getAppContext()
        {
            if (appContext == null)
            {
                appContext = new DemoApplicationContext();
            }
            return appContext;
        }

        /// <summary>
        /// Runs the form
        /// </summary>
        public void RunForm(Form form)
        {
            new Controller((ISpreadsheetWindow)form);
            // One more form is running
            formCount++;

            // When this form closes, we want to find out
            form.FormClosed += (o, e) => { if (--formCount <= 0) ExitThread(); };

            // Run the form
            form.Show();
        }
    }


    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Start an application context and run one form inside it
            DemoApplicationContext appContext = DemoApplicationContext.getAppContext();
            appContext.RunForm(new Spreadsheet());
            Application.Run(appContext);
        }
    }
}
