using System.Linq;
using HotelManagementSystem.Data;

namespace HotelManagementSystem.Services
{
    public class ProductService
    {
        private AppDbContext db = new AppDbContext();

        public object GetAll()
        {
            return db.Products.ToList();
        }
    }
}