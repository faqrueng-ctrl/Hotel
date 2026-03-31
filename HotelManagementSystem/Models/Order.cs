using System;
using System.Collections.Generic;

namespace HotelManagementSystem.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        public int UserId { get; set; }

        public DateTime OrderDate { get; set; }
        public decimal Total { get; set; }

        public virtual User User { get; set; }
        public virtual List<OrderItem> Items { get; set; }
    }
}