using System.Collections.Generic;

namespace HotelManagementSystem.Models
{
    public class Role
    {
        public int RoleId { get; set; }
        public string Name { get; set; }

        public virtual List<User> Users { get; set; }
    }
}