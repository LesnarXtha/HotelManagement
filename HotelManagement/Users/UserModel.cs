namespace HotelManagement.Users
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }  // Plain text password (not recommended for production)
    }

}
