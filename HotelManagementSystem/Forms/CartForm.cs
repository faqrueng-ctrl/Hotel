using HotelManagementSystem.Services;
using System;
using System.Windows.Forms;

public partial class Form1 : Form
{
    private void LoadCart()
    {
        dataGridView1.DataSource = new CartService().Get();
    }

    private void btnOrder_Click(object sender, EventArgs e)
    {
        new OrderService().Checkout();
        MessageBox.Show("Заказ оформлен");
    }
}