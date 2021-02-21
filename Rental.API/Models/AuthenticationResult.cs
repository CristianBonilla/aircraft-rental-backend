using System.Collections.Generic;

namespace Rental.API
{
    public class AuthenticationResult
    {
        public bool Success { get; set; }
        public string Token { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
