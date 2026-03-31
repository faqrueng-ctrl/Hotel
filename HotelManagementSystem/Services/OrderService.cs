using System.Linq;
using HotelManagementSystem.Data;
using HotelManagementSystem.Helpers;
using HotelManagementSystem.Models;

namespace HotelManagementSystem.Services
{
    public class OrderService
    {
        private readonly AppDbContext _context = new AppDbContext();

        public void CreateOrder()
        {
            var userId = Session.CurrentUser.UserId;

            var cart = _context.CartItems
                .Where(c => c.UserId == userId)
                .ToList();

            if (!cart.Any()) return;

            var order = new Order
            {
                UserId = userId,
                OrderDate = System.DateTime.Now,
                Total = 0
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            decimal total = 0;

            foreach (var item in cart)
            {
                var product = _context.Products.Find(item.ProductId);

                var orderItem = new OrderItem
                {
                    OrderId = order.OrderId,
                    ProductId = product.ProductId,
                    Quantity = item.Quantity,
                    Price = product.Price
                };

                total += product.Price * item.Quantity;

                _context.OrderItems.Add(orderItem);
            }

            order.Total = total;

            _context.CartItems.RemoveRange(cart);
            _context.SaveChanges();
        }
    }
}