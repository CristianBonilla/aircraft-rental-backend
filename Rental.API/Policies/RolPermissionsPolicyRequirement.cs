using Microsoft.AspNetCore.Authorization;

namespace Rental.API
{
    public class RolPermissionsPolicyRequirement : IAuthorizationRequirement
    {
        public string PermissionName { get; set; }

        public RolPermissionsPolicyRequirement(string permissionName) => (this.PermissionName) = (permissionName);
    }
}
