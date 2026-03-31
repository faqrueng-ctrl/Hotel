using System;
using System.Windows.Forms;
using HotelManagementSystem.Helpers;

namespace HotelManagementSystem.Forms
{
    public class MainForm : Form
    {
        public MainForm()
        {
            Text = "Hotel Management";
            Width = 500;
            Height = 260;
            StartPosition = FormStartPosition.CenterScreen;

            var lblUser = new Label
            {
                Left = 20,
                Top = 20,
                Width = 440,
                Text = Session.CurrentUser == null ? "Гость" : "Пользователь: " + Session.CurrentUser.Name + " (" + Session.CurrentUser.Role?.Name + ")"
            };

            var btnCatalog = new Button { Left = 20, Top = 60, Width = 140, Text = "Каталог" };
            btnCatalog.Click += (s, e) => new CatalogForm().ShowDialog();

            var btnCart = new Button { Left = 170, Top = 60, Width = 140, Text = "Корзина" };
            btnCart.Click += (s, e) => new CartForm().ShowDialog();

            var btnUsers = new Button { Left = 320, Top = 60, Width = 140, Text = "Пользователи" };
            btnUsers.Click += (s, e) => new UsersForm().ShowDialog();
            btnUsers.Visible = Session.IsAdmin;

            var btnLogout = new Button { Left = 20, Top = 100, Width = 140, Text = "Выход" };
            btnLogout.Click += (s, e) =>
            {
                Session.Logout();
                new LoginForm().Show();
                Close();
            };

            Controls.AddRange(new Control[] { lblUser, btnCatalog, btnCart, btnUsers, btnLogout });
        }
    }
}
