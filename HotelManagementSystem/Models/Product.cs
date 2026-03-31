using System.Collections.Generic;

namespace HotelManagementSystem.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }
        public decimal? OldPrice { get; set; }
        public int? DiscountPercent { get; set; }

        public int BrandId { get; set; }
        public int CategoryId { get; set; }

        public string Description { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual Category Category { get; set; }

        public virtual List<ProductImage> Images { get; set; }
    }
}