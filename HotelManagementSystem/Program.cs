using System;
using System.Threading;
using System.Windows.Forms;
using HotelManagementSystem.Forms;
using HotelManagementSystem.Helpers;

namespace HotelManagementSystem
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.ThreadException += (s, e) => ExceptionHandler.Handle(e.Exception);
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                var ex = e.ExceptionObject as Exception ?? new Exception("Неизвестная ошибка.");
                ExceptionHandler.Handle(ex);
            };
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginForm());
        }
    }
}
