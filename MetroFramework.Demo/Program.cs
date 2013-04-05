using System;
using System.Threading;
using System.Windows.Forms;

namespace MetroFramework.Demo
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Thread t1 = new Thread(RunMainForm);
            t1.Start("Thread 1");

            Thread t2 = new Thread(RunMainForm);
            t2.Start("Thread 2");

            t1.Join();
            t2.Join();
        }

        static void RunMainForm(object title)
        {
            
            MainForm form = new MainForm();
            form.Text += " (" + title + ")";
            Application.Run(form);
        }
    }
}
