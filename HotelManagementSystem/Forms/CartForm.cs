using System;
using System.Linq;
using System.Windows.Forms;
using HotelManagementSystem.Helpers;
using HotelManagementSystem.Services;

namespace HotelManagementSystem.Forms
{
    public class CartForm : Form
    {
        private readonly DataGridView grid = new DataGridView();
        private readonly CartService service = new CartService();

        public CartForm()
        {
            Text = "Корзина";
            Width = 760;
            Height = 460;
            StartPosition = FormStartPosition.CenterParent;

            grid.SetBounds(10, 10, 720, 360);
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.ReadOnly = true;

            var btnDelete = new Button { Left = 10, Top = 380, Width = 140, Text = "Удалить" };
            btnDelete.Click += BtnDelete_Click;

            var btnOrder = new Button { Left = 160, Top = 380, Width = 180, Text = "Оформить заказ" };
            btnOrder.Click += BtnOrder_Click;

            Controls.AddRange(new Control[] { grid, btnDelete, btnOrder });
            LoadCart();
        }

        private void LoadCart()
        {
            grid.DataSource = service.Get().Select(c => new
            {
                c.CartItemId,
                Product = c.Product.Name,
                c.Quantity,
                Price = c.Product.Price,
                Amount = c.Product.Price * c.Quantity
            }).ToList();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (grid.CurrentRow == null)
                {
                    return;
                }

                var id = Convert.ToInt32(grid.CurrentRow.Cells["CartItemId"].Value);
                service.Remove(id);
                LoadCart();
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
            }
        }

        private void BtnOrder_Click(object sender, EventArgs e)
        {
            try
            {
                new OrderService().Checkout();
                MessageBox.Show("Заказ оформлен.");
                LoadCart();
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
            }
        }
    }
}
