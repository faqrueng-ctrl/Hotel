using HotelManagementSystem.Services;
using System;
using System.Windows.Forms;
using System.Xml.Linq;

private void btnRegister_Click(object sender, EventArgs e)
{
    try
    {
        new AuthService().Register(
            txtName.Text,
            txtEmail.Text,
            txtPhone.Text,
            txtPassword.Text
        );

        MessageBox.Show("Регистрация успешна");
        new LoginForm().Show();
        this.Hide();
    }
    catch (Exception ex)
    {
        MessageBox.Show(ex.Message);
    }
}