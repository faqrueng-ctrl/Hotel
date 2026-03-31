using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HotelManagementSystem.Helpers;
using HotelManagementSystem.Services;

namespace HotelManagementSystem.Forms
{
    public class CatalogForm : Form
    {
        private readonly ProductService _service = new ProductService();
        private readonly DataGridView _grid = new DataGridView();
        private readonly TextBox _txtSearch = new TextBox();
        private readonly ComboBox _cmbSort = new ComboBox();

        public CatalogForm()
        {
            Text = "Каталог";
            Width = 980;
            Height = 540;
            StartPosition = FormStartPosition.CenterParent;

            Controls.Add(new Label { Left = 10, Top = 15, Width = 45, Text = "Поиск" });
            _txtSearch.SetBounds(60, 10, 260, 24);
            _txtSearch.TextChanged += (s, e) => LoadData();

            _cmbSort.SetBounds(330, 10, 180, 24);
            _cmbSort.Items.AddRange(new object[] { "Цена по возрастанию", "Цена по убыванию" });
            _cmbSort.SelectedIndex = 0;
            _cmbSort.SelectedIndexChanged += (s, e) => LoadData();

            var btnAddCart = new Button { Left = 520, Top = 10, Width = 150, Text = "Добавить в корзину" };
            btnAddCart.Click += AddToCart;

            var btnEdit = new Button { Left = 680, Top = 10, Width = 120, Text = "Добавить/Изм." };
            btnEdit.Click += EditProduct;
            btnEdit.Visible = Session.HasAtLeastManagerRights;

            var btnDelete = new Button { Left = 810, Top = 10, Width = 140, Text = "Удалить" };
            btnDelete.Click += DeleteProduct;
            btnDelete.Visible = Session.IsAdmin;

            _grid.SetBounds(10, 45, 940, 445);
            _grid.ReadOnly = true;
            _grid.AutoGenerateColumns = true;
            _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _grid.CellFormatting += Grid_CellFormatting;

            Controls.AddRange(new Control[] { _txtSearch, _cmbSort, btnAddCart, btnEdit, btnDelete, _grid });
            LoadData();
        }

        private void LoadData()
        {
            var asc = _cmbSort.SelectedIndex != 1;
            _grid.DataSource = _service.GetCatalog(_txtSearch.Text, null, asc)
                .Select(p => new
                {
                    p.ProductId,
                    p.Name,
                    p.Price,
                    p.OldPrice,
                    p.DiscountPercent,
                    NewPrice = p.OldPrice.HasValue && p.DiscountPercent.HasValue
                        ? p.OldPrice.Value * (100 - p.DiscountPercent.Value) / 100
                        : p.Price,
                    Brand = p.Brand?.Name,
                    Category = p.Category?.Name,
                    p.Description,
                    p.ImagePath
                })
                .ToList();
        }

        private int? GetSelectedProductId()
        {
            if (_grid.CurrentRow == null)
            {
                return null;
            }

            return Convert.ToInt32(_grid.CurrentRow.Cells["ProductId"].Value);
        }

        private void AddToCart(object sender, EventArgs e)
        {
            try
            {
                var id = GetSelectedProductId();
                if (!id.HasValue)
                {
                    return;
                }

                new CartService().AddToCart(id.Value);
                MessageBox.Show("Товар добавлен в корзину.");
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
            }
        }

        private void EditProduct(object sender, EventArgs e)
        {
            var id = GetSelectedProductId();
            using (var form = new ProductEditForm(id))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadData();
                }
            }
        }

        private void DeleteProduct(object sender, EventArgs e)
        {
            try
            {
                var id = GetSelectedProductId();
                if (!id.HasValue)
                {
                    return;
                }

                _service.Delete(id.Value);
                LoadData();
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
            }
        }

        private void Grid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (_grid.Columns[e.ColumnIndex].Name == "Price")
            {
                var row = _grid.Rows[e.RowIndex];
                var value = row.Cells["Price"].Value;
                if (value != null && decimal.TryParse(value.ToString(), out var price) && price > 1000)
                {
                    row.DefaultCellStyle.BackColor = Color.MistyRose;
                    row.DefaultCellStyle.Font = new Font(_grid.Font, FontStyle.Bold);
                }
            }
        }
    }
}
