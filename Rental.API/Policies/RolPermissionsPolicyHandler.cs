using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Rental.API
{
    public class RolPermissionsPolicyHandler : AuthorizationHandler<RolPermissionsPolicyRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RolPermissionsPolicyRequirement requirement)
        {
            var claims = context.User?.Claims ?? Enumerable.Empty<Claim>();
            bool hasRole = claims.Any(c => c.Type == ClaimTypes.Role);
            bool hasPermission = claims.Where(c => c.Type == "permissions")
                .Select(p => p.Value)
                .Any(permission => permission == requirement.PermissionName);
            if (hasRole && hasPermission)
            {
                context.Succeed(requirement);

                return Task.CompletedTask;
            }
            context.Fail();

            return Task.CompletedTask;
        }
    }
}
