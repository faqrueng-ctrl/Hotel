using System;
using System.Windows.Forms;
using HotelManagementSystem.Services;
using HotelManagementSystem.Helpers;

namespace HotelManagementSystem.Forms
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var service = new AuthService();

            var user = service.Login(txtLogin.Text, txtPassword.Text);

            if (user == null)
            {
                MessageBox.Show("Ошибка входа");
                return;
            }

            Session.CurrentUser = user;

            new MainForm().Show();
            this.Hide();
        }
    }
}