using HotelManagementSystem.Forms;
using System;
using System.Windows.Forms;

public partial class Form1 : Form
{
    private void btnProducts_Click(object sender, EventArgs e)
    {
        new CatalogForm().Show();
    }

    private void btnUsers_Click(object sender, EventArgs e)
    {
        new UsersForm().Show();
    }
}