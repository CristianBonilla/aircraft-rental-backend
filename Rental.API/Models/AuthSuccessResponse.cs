namespace Rental.API
{
    public class AuthSuccessResponse
    {
        public string Token { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }
}
