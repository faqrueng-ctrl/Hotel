using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using HotelManagementSystem.Data;
using HotelManagementSystem.Helpers;
using HotelManagementSystem.Models;

namespace HotelManagementSystem.Services
{
    public class CartService
    {
        public List<CartItem> Get()
        {
            EnsureUser();
            using (var db = new AppDbContext())
            {
                return db.CartItems
                    .Include(c => c.Product)
                    .Where(c => c.UserId == Session.CurrentUser.UserId)
                    .ToList();
            }
        }

        public void AddToCart(int productId)
        {
            EnsureUser();
            using (var db = new AppDbContext())
            {
                var userId = Session.CurrentUser.UserId;
                var item = db.CartItems.FirstOrDefault(c => c.ProductId == productId && c.UserId == userId);

                if (item != null)
                {
                    item.Quantity++;
                }
                else
                {
                    db.CartItems.Add(new CartItem
                    {
                        ProductId = productId,
                        UserId = userId,
                        Quantity = 1
                    });
                }

                db.SaveChanges();
            }
        }

        public void Remove(int cartItemId)
        {
            EnsureUser();
            using (var db = new AppDbContext())
            {
                var item = db.CartItems.FirstOrDefault(c => c.CartItemId == cartItemId && c.UserId == Session.CurrentUser.UserId);
                if (item == null)
                {
                    return;
                }

                db.CartItems.Remove(item);
                db.SaveChanges();
            }
        }

        private static void EnsureUser()
        {
            if (Session.CurrentUser == null)
            {
                throw new InvalidOperationException("Требуется авторизация.");
            }
        }
    }
}
