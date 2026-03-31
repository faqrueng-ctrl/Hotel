using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using HotelManagementSystem.Data;
using HotelManagementSystem.Helpers;
using HotelManagementSystem.Models;

namespace HotelManagementSystem.Services
{
    public class UserService
    {
        public List<User> GetAll()
        {
            using (var db = new AppDbContext())
            {
                return db.Users.Include(u => u.Role).OrderBy(u => u.Name).ToList();
            }
        }

        public List<Role> GetRoles()
        {
            using (var db = new AppDbContext())
            {
                return db.Roles.OrderBy(r => r.Name).ToList();
            }
        }

        public void UpdateProfile(User profile)
        {
            if (Session.CurrentUser == null)
            {
                throw new InvalidOperationException("Требуется авторизация.");
            }

            using (var db = new AppDbContext())
            {
                var dbUser = db.Users.FirstOrDefault(u => u.UserId == Session.CurrentUser.UserId);
                if (dbUser == null)
                {
                    throw new InvalidOperationException("Пользователь не найден.");
                }

                dbUser.Name = profile.Name;
                dbUser.Email = profile.Email;
                dbUser.Phone = profile.Phone;
                db.SaveChanges();
            }
        }

        public void UpdateByAdmin(User user)
        {
            if (!Session.IsAdmin)
            {
                throw new UnauthorizedAccessException("Только администратор может изменять роли пользователей.");
            }

            using (var db = new AppDbContext())
            {
                var dbUser = db.Users.FirstOrDefault(u => u.UserId == user.UserId);
                if (dbUser == null)
                {
                    throw new InvalidOperationException("Пользователь не найден.");
                }

                dbUser.Name = user.Name;
                dbUser.Email = user.Email;
                dbUser.Phone = user.Phone;
                dbUser.RoleId = user.RoleId;
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            if (!Session.IsAdmin)
            {
                throw new UnauthorizedAccessException("Только администратор может удалять пользователей.");
            }

            using (var db = new AppDbContext())
            {
                var user = db.Users.FirstOrDefault(u => u.UserId == id);
                if (user == null)
                {
                    return;
                }

                db.Users.Remove(user);
                db.SaveChanges();
            }
        }
    }
}
