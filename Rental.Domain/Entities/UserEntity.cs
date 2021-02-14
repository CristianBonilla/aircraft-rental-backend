namespace Rental.Domain
{
    public class UserEntity
    {
        public int Id { get; set; }
        public int IdRole { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public RoleEntity Role { get; set; }
    }
}
