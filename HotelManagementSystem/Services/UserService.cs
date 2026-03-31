using System.Linq;
using HotelManagementSystem.Data;
using HotelManagementSystem.Models;

namespace HotelManagementSystem.Services
{
    public class UserService
    {
        private readonly AppDbContext _context = new AppDbContext();

        public IQueryable<User> GetAll()
        {
            return _context.Users.Include("Role");
        }

        public void Update(User user)
        {
            var dbUser = _context.Users.Find(user.UserId);

            dbUser.Name = user.Name;
            dbUser.Email = user.Email;
            dbUser.Phone = user.Phone;

            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var user = _context.Users.Find(id);
            _context.Users.Remove(user);
            _context.SaveChanges();
        }
    }
}