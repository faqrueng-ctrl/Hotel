using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using HotelManagementSystem.Data;
using HotelManagementSystem.Helpers;
using HotelManagementSystem.Models;

namespace HotelManagementSystem.Services
{
    public class OrderService
    {
        public void Checkout()
        {
            if (Session.CurrentUser == null)
            {
                throw new InvalidOperationException("Требуется авторизация.");
            }

            using (var db = new AppDbContext())
            {
                var userId = Session.CurrentUser.UserId;
                var cart = db.CartItems
                    .Include(c => c.Product)
                    .Where(c => c.UserId == userId)
                    .ToList();

                if (!cart.Any())
                {
                    throw new InvalidOperationException("Корзина пуста.");
                }

                var order = new Order
                {
                    UserId = userId,
                    OrderDate = DateTime.Now,
                    Total = 0m
                };

                db.Orders.Add(order);
                db.SaveChanges();

                decimal total = 0m;
                foreach (var item in cart)
                {
                    var price = item.Product.Price;
                    db.OrderItems.Add(new OrderItem
                    {
                        OrderId = order.OrderId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = price
                    });

                    total += price * item.Quantity;
                }

                order.Total = total;
                db.CartItems.RemoveRange(cart);
                db.SaveChanges();
            }
        }

        public List<Order> GetMyOrders()
        {
            if (Session.CurrentUser == null)
            {
                return new List<Order>();
            }

            using (var db = new AppDbContext())
            {
                var userId = Session.CurrentUser.UserId;
                return db.Orders.Where(o => o.UserId == userId).OrderByDescending(o => o.OrderDate).ToList();
            }
        }
    }
}
