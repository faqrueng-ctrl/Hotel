using HotelManagementSystem.Data;
using HotelManagementSystem.Helpers;
using HotelManagementSystem.Models;
using System;
using System.Linq;

namespace HotelManagementSystem.Services
{
    public class CartService
    {
        private readonly AppDbContext _context;

        public CartService()
        {
            _context = new AppDbContext();
        }

        public void AddToCart(int productId)
        {
            var userId = Session.CurrentUser.UserId;

            var item = _context.CartItems
                .FirstOrDefault(c => c.ProductId == productId && c.UserId == userId);

            if (item != null)
                item.Quantity++;
            else
            {
                _context.CartItems.Add(new CartItem
                {
                    ProductId = productId,
                    UserId = userId,
                    Quantity = 1
                });
            }

            _context.SaveChanges();
        }
    }
}