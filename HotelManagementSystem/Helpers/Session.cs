using HotelManagementSystem.Models;

namespace HotelManagementSystem.Helpers
{
    public static class Session
    {
        public static User CurrentUser { get; set; }

        public static bool IsAuthenticated => CurrentUser != null;
        public static bool IsUser => CurrentUser?.Role?.Name == "User";
        public static bool IsManager => CurrentUser?.Role?.Name == "Manager";
        public static bool IsAdmin => CurrentUser?.Role?.Name == "Admin";

        public static bool HasAtLeastManagerRights => IsManager || IsAdmin;

        public static void Logout()
        {
            CurrentUser = null;
        }
    }
}
