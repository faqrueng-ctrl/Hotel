using HotelManagementSystem.Models;

namespace HotelManagementSystem.Helpers
{
    public static class Session
    {
        public static User CurrentUser { get; set; }

        public static bool IsAdmin => CurrentUser?.Role?.Name == "Admin";
    }
}