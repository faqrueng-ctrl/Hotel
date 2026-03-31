using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using HotelManagementSystem.Data;
using HotelManagementSystem.Models;

namespace HotelManagementSystem.Services
{
    public class ProductService
    {
        public List<Product> GetCatalog(string search = null, int? categoryId = null, bool sortByPriceAsc = true)
        {
            using (var db = new AppDbContext())
            {
                var query = db.Products
                    .Include(p => p.Brand)
                    .Include(p => p.Category)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(p => p.Name.Contains(search) || p.Description.Contains(search));
                }

                if (categoryId.HasValue)
                {
                    query = query.Where(p => p.CategoryId == categoryId.Value);
                }

                query = sortByPriceAsc ? query.OrderBy(p => p.Price) : query.OrderByDescending(p => p.Price);
                return query.ToList();
            }
        }

        public List<Category> GetCategories()
        {
            using (var db = new AppDbContext())
            {
                return db.Categories.OrderBy(c => c.Name).ToList();
            }
        }

        public List<Brand> GetBrands()
        {
            using (var db = new AppDbContext())
            {
                return db.Brands.OrderBy(c => c.Name).ToList();
            }
        }

        public Product GetById(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.Products.FirstOrDefault(p => p.ProductId == id);
            }
        }

        public void Create(Product product)
        {
            ValidateProduct(product);
            using (var db = new AppDbContext())
            {
                db.Products.Add(product);
                db.SaveChanges();
            }
        }

        public void Update(Product product)
        {
            ValidateProduct(product);
            using (var db = new AppDbContext())
            {
                var dbProduct = db.Products.FirstOrDefault(p => p.ProductId == product.ProductId);
                if (dbProduct == null)
                {
                    throw new InvalidOperationException("Товар не найден.");
                }

                dbProduct.Name = product.Name;
                dbProduct.Price = product.Price;
                dbProduct.OldPrice = product.OldPrice;
                dbProduct.DiscountPercent = product.DiscountPercent;
                dbProduct.BrandId = product.BrandId;
                dbProduct.CategoryId = product.CategoryId;
                dbProduct.Description = product.Description;
                dbProduct.ImagePath = product.ImagePath;
                db.SaveChanges();
            }
        }

        public void Delete(int productId)
        {
            using (var db = new AppDbContext())
            {
                var hasOrderItems = db.OrderItems.Any(oi => oi.ProductId == productId);
                if (hasOrderItems)
                {
                    throw new InvalidOperationException("Невозможно удалить товар, так как он присутствует в одном или нескольких заказах.");
                }

                var product = db.Products.FirstOrDefault(p => p.ProductId == productId);
                if (product == null)
                {
                    return;
                }

                db.Products.Remove(product);
                db.SaveChanges();
            }
        }

        public string SaveProductImage(string sourceFilePath)
        {
            if (string.IsNullOrWhiteSpace(sourceFilePath) || !File.Exists(sourceFilePath))
            {
                throw new ArgumentException("Файл изображения не найден.");
            }

            var projectRoot = AppDomain.CurrentDomain.BaseDirectory;
            var imagesDirectory = Path.Combine(projectRoot, "ProductImages");
            Directory.CreateDirectory(imagesDirectory);

            var fileName = Guid.NewGuid().ToString("N") + Path.GetExtension(sourceFilePath);
            var destination = Path.Combine(imagesDirectory, fileName);
            File.Copy(sourceFilePath, destination, true);

            return Path.Combine("ProductImages", fileName);
        }

        private static void ValidateProduct(Product product)
        {
            if (product == null)
            {
                throw new ArgumentException("Товар не задан.");
            }

            if (string.IsNullOrWhiteSpace(product.Name))
            {
                throw new ArgumentException("Название товара обязательно.");
            }

            if (product.Price < 0)
            {
                throw new ArgumentException("Цена не может быть отрицательной.");
            }

            if (product.DiscountPercent.HasValue && (product.DiscountPercent.Value < 0 || product.DiscountPercent.Value > 100))
            {
                throw new ArgumentException("Скидка должна быть в диапазоне 0-100.");
            }
        }
    }
}
