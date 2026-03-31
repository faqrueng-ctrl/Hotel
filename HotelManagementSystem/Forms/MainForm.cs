using System;
using System.Windows.Forms;
using HotelManagementSystem.Helpers;

namespace HotelManagementSystem.Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            lblUser.Text = Session.CurrentUser.Name;

            if (!Session.IsAdmin)
                btnUsers.Visible = false;
        }

        private void btnCatalog_Click(object sender, EventArgs e)
        {
            new CatalogForm().Show();
        }
    }
}