using System.Collections.Generic;
using Rental.Domain;

namespace Rental.API
{
    public class AuthSuccessResponse
    {
        public string Token { get; set; }
        public UserResponse User { get; set; }
        public RoleResponse Role { get; set; }
        public ICollection<PermissionEntity> Permissions { get; set; }
    }
}
