using System;
using System.Data.Entity;
using System.Linq;
using HotelManagementSystem.Data;
using HotelManagementSystem.Helpers;
using HotelManagementSystem.Models;

namespace HotelManagementSystem.Services
{
    public class AuthService
    {
        public User Login(string login, string password)
        {
            if (!Validator.IsRequired(login) || !Validator.IsRequired(password))
            {
                throw new ArgumentException("Введите логин и пароль.");
            }

            using (var db = new AppDbContext())
            {
                return db.Users
                    .Include(u => u.Role)
                    .FirstOrDefault(u =>
                        (u.Email == login || u.Phone == login) &&
                        u.Password == password);
            }
        }

        public User Register(string name, string email, string phone, string password)
        {
            if (!Validator.IsRequired(name))
            {
                throw new ArgumentException("Имя обязательно.");
            }

            if (!Validator.IsValidEmail(email))
            {
                throw new ArgumentException("Некорректный email.");
            }

            if (!Validator.IsValidPhone(phone))
            {
                throw new ArgumentException("Некорректный телефон.");
            }

            if (!Validator.IsValidPassword(password))
            {
                throw new ArgumentException("Пароль должен содержать не менее 6 символов.");
            }

            using (var db = new AppDbContext())
            {
                if (db.Users.Any(u => u.Email == email))
                {
                    throw new ArgumentException("Пользователь с таким email уже существует.");
                }

                if (db.Users.Any(u => u.Phone == phone))
                {
                    throw new ArgumentException("Пользователь с таким телефоном уже существует.");
                }

                var userRole = db.Roles.FirstOrDefault(r => r.Name == "User");
                if (userRole == null)
                {
                    throw new InvalidOperationException("В БД не найдена роль User.");
                }

                var user = new User
                {
                    Name = name.Trim(),
                    Email = email.Trim(),
                    Phone = phone.Trim(),
                    Password = password,
                    RoleId = userRole.RoleId
                };

                db.Users.Add(user);
                db.SaveChanges();
                return user;
            }
        }
    }
}
