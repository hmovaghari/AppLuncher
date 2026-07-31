using System;
using System.Windows.Forms;

namespace AppLuncher
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += delegate(object sender, System.Threading.ThreadExceptionEventArgs args)
            {
                MessageBox.Show(args.Exception.Message, "Unexpected error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            };

            Application.Run(new Form1());
        }
    }
}
