namespace Rental.API
{
    public class UserResponse
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public long IdentificationDocument { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
