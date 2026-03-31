using System;
using System.Windows.Forms;
using HotelManagementSystem.Services;

namespace HotelManagementSystem.Forms
{
    public partial class CatalogForm : Form
    {
        public CatalogForm()
        {
            InitializeComponent();
        }

        private void CatalogForm_Load(object sender, EventArgs e)
        {
            var service = new ProductService();
            dataGridView1.DataSource = service.GetAll();
        }
    }
}