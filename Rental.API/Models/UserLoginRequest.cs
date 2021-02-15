namespace Rental.API
{
    public enum UserLoginType
    {
        Username,
        Email
    }

    public class UserLoginRequest
    {
        public UserLoginType Type { get; set; }
        public string UsernameOrEmail { get; set; }
        public string Password { get; set; }
    }
}
