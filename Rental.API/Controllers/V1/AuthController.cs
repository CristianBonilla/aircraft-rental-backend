using Microsoft.AspNetCore.Mvc;
using Rental.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        [HttpPost(ApiRoutes.Auth.CreateRole)]
        public async Task<IActionResult> Post([FromBody] RoleEntity role, int[] permissionsIDs)
        {
            if (!permissionsIDs.Any())
                return BadRequest();
            RoleEntity roleCreated = await authService.CreateRole(role, permissionsIDs);

            return Ok(roleCreated);
        }
    }
}
