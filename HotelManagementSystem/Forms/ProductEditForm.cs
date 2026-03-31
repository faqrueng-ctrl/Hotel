using System;
using System.Linq;
using System.Windows.Forms;
using HotelManagementSystem.Helpers;
using HotelManagementSystem.Models;
using HotelManagementSystem.Services;

namespace HotelManagementSystem.Forms
{
    public class ProductEditForm : Form
    {
        private readonly int? _productId;
        private readonly ProductService _service = new ProductService();

        private readonly TextBox txtName = new TextBox();
        private readonly TextBox txtPrice = new TextBox();
        private readonly TextBox txtOldPrice = new TextBox();
        private readonly TextBox txtDiscount = new TextBox();
        private readonly TextBox txtDescription = new TextBox();
        private readonly TextBox txtImagePath = new TextBox();
        private readonly ComboBox cmbBrand = new ComboBox();
        private readonly ComboBox cmbCategory = new ComboBox();

        public ProductEditForm(int? productId = null)
        {
            _productId = productId;
            Text = _productId.HasValue ? "Редактирование товара" : "Добавление товара";
            Width = 520;
            Height = 420;
            StartPosition = FormStartPosition.CenterParent;

            AddRow("Название", txtName, 20);
            AddRow("Цена", txtPrice, 55);
            AddRow("Старая цена", txtOldPrice, 90);
            AddRow("Скидка %", txtDiscount, 125);
            AddRow("Бренд", cmbBrand, 160);
            AddRow("Категория", cmbCategory, 195);
            AddRow("Описание", txtDescription, 230);
            AddRow("Изображение", txtImagePath, 265);

            var btnImage = new Button { Left = 410, Top = 265, Width = 90, Text = "Выбрать" };
            btnImage.Click += BtnImage_Click;

            var btnSave = new Button { Left = 200, Top = 310, Width = 120, Text = "Сохранить" };
            btnSave.Click += BtnSave_Click;

            Controls.AddRange(new Control[] { btnImage, btnSave });
            LoadLookups();
            LoadProductIfNeeded();
        }

        private void AddRow(string label, Control control, int top)
        {
            Controls.Add(new Label { Left = 20, Top = top + 4, Width = 110, Text = label });
            control.SetBounds(140, top, 260, 24);
            Controls.Add(control);
        }

        private void LoadLookups()
        {
            cmbBrand.DataSource = _service.GetBrands();
            cmbBrand.DisplayMember = "Name";
            cmbBrand.ValueMember = "BrandId";

            cmbCategory.DataSource = _service.GetCategories();
            cmbCategory.DisplayMember = "Name";
            cmbCategory.ValueMember = "CategoryId";
        }

        private void LoadProductIfNeeded()
        {
            if (!_productId.HasValue)
            {
                return;
            }

            var product = _service.GetById(_productId.Value);
            if (product == null)
            {
                return;
            }

            txtName.Text = product.Name;
            txtPrice.Text = product.Price.ToString("0.##");
            txtOldPrice.Text = product.OldPrice?.ToString("0.##");
            txtDiscount.Text = product.DiscountPercent?.ToString();
            txtDescription.Text = product.Description;
            txtImagePath.Text = product.ImagePath;
            cmbBrand.SelectedValue = product.BrandId;
            cmbCategory.SelectedValue = product.CategoryId;
        }

        private void BtnImage_Click(object sender, EventArgs e)
        {
            try
            {
                using (var ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Изображения|*.png;*.jpg;*.jpeg;*.bmp";
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtImagePath.Text = _service.SaveProductImage(ofd.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var product = new Product
                {
                    ProductId = _productId ?? 0,
                    Name = txtName.Text,
                    Price = decimal.Parse(txtPrice.Text),
                    OldPrice = string.IsNullOrWhiteSpace(txtOldPrice.Text) ? (decimal?)null : decimal.Parse(txtOldPrice.Text),
                    DiscountPercent = string.IsNullOrWhiteSpace(txtDiscount.Text) ? (int?)null : int.Parse(txtDiscount.Text),
                    BrandId = Convert.ToInt32(cmbBrand.SelectedValue),
                    CategoryId = Convert.ToInt32(cmbCategory.SelectedValue),
                    Description = txtDescription.Text,
                    ImagePath = txtImagePath.Text
                };

                if (_productId.HasValue)
                {
                    _service.Update(product);
                }
                else
                {
                    _service.Create(product);
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
            }
        }
    }
}
