using System.Collections.Generic;

namespace HotelManagementSystem.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }

        public int RoleId { get; set; }

        public virtual Role Role { get; set; }

        public virtual List<CartItem> CartItems { get; set; }
        public virtual List<Order> Orders { get; set; }
    }
}