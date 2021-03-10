using Microsoft.AspNetCore.Authorization;

namespace Rental.API
{
    public class RolePermissionsPolicyRequirement : IAuthorizationRequirement
    {
        public string PermissionName { get; set; }

        public RolePermissionsPolicyRequirement(string permissionName) => (this.PermissionName) = (permissionName);
    }
}
