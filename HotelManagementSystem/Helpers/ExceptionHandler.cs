using System;
using System.Windows.Forms;

namespace HotelManagementSystem.Helpers
{
    public static class ExceptionHandler
    {
        public static void Handle(Exception ex)
        {
            MessageBox.Show(
                "Ошибка: " + ex.Message,
                "Ошибка",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}