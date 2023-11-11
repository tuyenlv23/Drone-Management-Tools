using DevExpress.XtraLayout;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Drone_Management_Tools
{
    internal static class Program
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool createNew = true;
            using (Mutex mutex = new Mutex(true, "Drone Management Tools", out createNew))
            {
                if (createNew)
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    LayoutControl.AllowCustomizationDefaultValue = false;
                    Application.Run(new UIMain());
                }
                else
                {
                    System.Diagnostics.Process currentProcess = System.Diagnostics.Process.GetCurrentProcess();
                    foreach (var process in System.Diagnostics.Process.GetProcessesByName(currentProcess.ProcessName))
                    {
                        if (process.Id != currentProcess.Id)
                        {
                            SetForegroundWindow(process.MainWindowHandle);
                            MessageBox.Show("Another instance of the application is ready running");
                            break;
                        }
                    }
                }
            }
        }
    }
}
