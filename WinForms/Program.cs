using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DnD4e.CombatManager.Test {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main () {
            // Add the event handler for handling UI thread exceptions to the event.
            Application.ThreadException += NBug.Handler.ThreadException;

            // Set the unhandled exception mode to force all Windows Forms errors
            // to go through our handler.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            // Add the event handler for handling non-UI thread exceptions to the event. 
            AppDomain.CurrentDomain.UnhandledException += NBug.Handler.UnhandledException;

            // Add the event handler for handling unobserved async based Task exceptions.
            TaskScheduler.UnobservedTaskException += NBug.Handler.UnobservedTaskException;

#if !DEBUG
            NBug.Settings.ReleaseMode = true;
#endif

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LibraryForm());
        }
    }
}
