using System;
using System.Windows.Forms;
using HotelManagementSystem.Helpers;
using HotelManagementSystem.Services;

namespace HotelManagementSystem.Forms
{
    public class LoginForm : Form
    {
        private readonly TextBox txtLogin = new TextBox();
        private readonly TextBox txtPassword = new TextBox();

        public LoginForm()
        {
            Text = "Вход";
            Width = 360;
            Height = 220;
            StartPosition = FormStartPosition.CenterScreen;

            var lblLogin = new Label { Left = 20, Top = 20, Width = 120, Text = "Email / Телефон" };
            txtLogin.SetBounds(150, 20, 170, 24);
            var lblPassword = new Label { Left = 20, Top = 60, Width = 120, Text = "Пароль" };
            txtPassword.SetBounds(150, 60, 170, 24);
            txtPassword.PasswordChar = '*';

            var btnLogin = new Button { Left = 150, Top = 100, Width = 80, Text = "Войти" };
            btnLogin.Click += BtnLogin_Click;

            var btnRegister = new Button { Left = 240, Top = 100, Width = 80, Text = "Регистрация" };
            btnRegister.Click += (s, e) =>
            {
                new RegisterForm().Show();
                Hide();
            };

            Controls.AddRange(new Control[] { lblLogin, txtLogin, lblPassword, txtPassword, btnLogin, btnRegister });
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                var user = new AuthService().Login(txtLogin.Text, txtPassword.Text);
                if (user == null)
                {
                    MessageBox.Show("Неверный логин или пароль.");
                    return;
                }

                Session.CurrentUser = user;
                new MainForm().Show();
                Hide();
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
            }
        }
    }
}
