using System.Linq;
using HotelManagementSystem.Data;
using HotelManagementSystem.Models;
using System.Data.Entity;

namespace HotelManagementSystem.Services
{
    public class AuthService
    {
        private AppDbContext db = new AppDbContext();

        public User Login(string login, string password)
        {
            return db.Users
                .Include(u => u.Role)
                .FirstOrDefault(u =>
                    (u.Email == login || u.Phone == login)
                    && u.Password == password);
        }
    }
}