using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Rental.Domain;

namespace Rental.API.Controllers.V1
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        readonly IAuthService authService;

        public AuthController(IAuthService authService) =>
            (this.authService) = (authService);

        [HttpGet(ApiRoutes.Auth.GetRoles)]
        public async IAsyncEnumerable<RoleEntity> Get()
        {
            var roles = authService.Roles();
            await foreach (RoleEntity role in roles)
                yield return role;
        }

        [HttpGet(ApiRoutes.Auth.GetRoleById)]
        public async Task<IActionResult> Get(int id)
        {
            RoleEntity role = await authService.RoleById(id);
            if (role == null)
                return NotFound();

            return Ok(role);
        }

        [HttpGet(ApiRoutes.Auth.GetUsers)]
        public async IAsyncEnumerable<UserEntity> GetUsers()
        {
            var users = authService.Users();
            await foreach (UserEntity user in users)
                yield return user;
        }

        [HttpGet(ApiRoutes.Auth.GetUserById)]
        public async Task<IActionResult> GetUserById(int id)
        {
            UserEntity user = await authService.UserById(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        //[HttpPost(ApiRoutes.Auth.CreateRole)]
        //public async Task<IActionResult> Post([FromBody] RoleRequest role)
        //{
        //    if (!role.PermissionsIDs.Any())
        //        return BadRequest();
        //    RoleEntity roleCreated = await authService.CreateRole(new RoleEntity { Name = role.Name }, role.PermissionsIDs);

        //    return Ok(roleCreated);
        //}
    }
}
