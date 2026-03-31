using System;
using System.Windows.Forms;
using HotelManagementSystem.Helpers;
using HotelManagementSystem.Services;

namespace HotelManagementSystem.Forms
{
    public class RegisterForm : Form
    {
        private readonly TextBox txtName = new TextBox();
        private readonly TextBox txtEmail = new TextBox();
        private readonly TextBox txtPhone = new TextBox();
        private readonly TextBox txtPassword = new TextBox();

        public RegisterForm()
        {
            Text = "Регистрация";
            Width = 420;
            Height = 280;
            StartPosition = FormStartPosition.CenterScreen;

            AddRow("ФИО", txtName, 20);
            AddRow("Email", txtEmail, 60);
            AddRow("Телефон", txtPhone, 100);
            AddRow("Пароль", txtPassword, 140);
            txtPassword.PasswordChar = '*';

            var btnRegister = new Button { Left = 180, Top = 190, Width = 100, Text = "Создать" };
            btnRegister.Click += BtnRegister_Click;
            Controls.Add(btnRegister);
        }

        private void AddRow(string label, Control control, int top)
        {
            Controls.Add(new Label { Left = 20, Top = top + 4, Width = 150, Text = label });
            control.SetBounds(180, top, 200, 24);
            Controls.Add(control);
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                new AuthService().Register(txtName.Text, txtEmail.Text, txtPhone.Text, txtPassword.Text);
                MessageBox.Show("Регистрация успешна.");
                new LoginForm().Show();
                Hide();
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
            }
        }
    }
}
