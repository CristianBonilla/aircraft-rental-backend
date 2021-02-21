using System.Collections.Generic;
using Rental.Domain;

namespace Rental.API
{
    public class AuthenticationResult
    {
        public bool Success { get; set; }
        public string Token { get; set; }
        public UserEntity User { get; set; }
        public RoleEntity Role { get; set; }
        public ICollection<PermissionEntity> Permissions { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
