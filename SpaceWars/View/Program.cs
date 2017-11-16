using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SpaceWarsView;
using SpaceWarsControl;


namespace SpaceWars
{
    static class Program
    {

        /// <summary>
        /// Keeps track of how many top-level forms are running
        /// </summary>
        class SpaceWarsContext : ApplicationContext
        {
            // Number of open forms
            private int formCount = 0;

            // Singleton ApplicationContext
            private static SpaceWarsContext appContext;

            /// <summary>
            /// Private constructor for singleton pattern
            /// </summary>
            private SpaceWarsContext()
            {

            }

            /// <summary>
            /// Returns the one DemoApplicationContext.
            /// </summary>
            public static SpaceWarsContext getAppContext()
            {
                if (appContext == null)
                {
                    appContext = new SpaceWarsContext();
                }
                return appContext;
            }

            /// <summary>
            /// Runs the form
            /// </summary>
            public void RunForm(Form form)
            {
                new Controller((ISpaceWarsWindow)form);
                // One more form is running
                formCount++;

                // When this form closes, we want to find out
                form.FormClosed += (o, e) => { if (--formCount <= 0) ExitThread(); };

                // Run the form
                form.Show();
            }



        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Start an application context and run one form inside it
            SpaceWarsContext appContext = SpaceWarsContext.getAppContext();
            appContext.RunForm(new SpaceWarsForm());

            Application.Run(appContext);
        }
    }
}
