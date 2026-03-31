namespace HotelManagementSystem.Models
{
    public class ProductImage
    {
        public int ProductImageId { get; set; }
        public int ProductId { get; set; }

        public string ImagePath { get; set; }

        public virtual Product Product { get; set; }
    }
}