using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AutoMapper;
using Rental.Domain;

namespace Rental.API.Controllers.V1
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class IdentityController : ControllerBase
    {
        readonly IMapper mapper;
        readonly IAuthService authService;
        readonly IIdentityService identityService;

        public IdentityController(
            IMapper mapper,
            IAuthService authService,
            IIdentityService identityService)
        {
            this.mapper = mapper;
            this.authService = authService;
            this.identityService = identityService;
        }
            
        [AllowAnonymous]
        [HttpPost(ApiRoutes.Identity.Register)]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest userRegisterRequest)
        {
            UserEntity user = mapper.Map<UserEntity>(userRegisterRequest);
            AuthenticationResult authResponse = await identityService.Register(user, userRegisterRequest.Role);

            await Task.Delay(3000);

            if (!authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = authResponse.Errors
                });
            }
            UserResponse userResponse = mapper.Map<UserResponse>(authResponse.User);
            RoleResponse roleResponse = mapper.Map<RoleResponse>(authResponse.Role);

            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token,
                User = userResponse,
                Role = roleResponse,
                Permissions = authResponse.Permissions
            });
        }

        [AllowAnonymous]
        [HttpPost(ApiRoutes.Identity.Login)]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest userLoginRequest)
        {
            AuthenticationResult authResponse = await identityService.Login(userLoginRequest);

            await Task.Delay(3000);

            if (!authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = authResponse.Errors
                });
            }
            UserResponse userResponse = mapper.Map<UserResponse>(authResponse.User);
            RoleResponse roleResponse = mapper.Map<RoleResponse>(authResponse.Role);

            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token,
                User = userResponse,
                Role = roleResponse,
                Permissions = authResponse.Permissions
            });
        }

        [Authorize(Policy = "RolesPolicy")]
        [HttpPost(ApiRoutes.Identity.CreateRole)]
        public async Task<IActionResult> CreateRole([FromBody] RoleRequest roleRequest)
        {
            bool exstingRole = await authService.RoleExists(r => r.Name == roleRequest.Name);
            if (exstingRole)
                return BadRequest(new { Errors = new[] { "The role is already registered" } });
            if (!roleRequest.PermissionsIDs.Any())
                return BadRequest(new { Errors = new[] { "The permissions to grant the role were not defined" } });
            RoleEntity role = mapper.Map<RoleEntity>(roleRequest);
            RoleEntity roleCreated = await authService.CreateRole(role, roleRequest.PermissionsIDs);

            return Ok(roleCreated);
        }

        [Authorize(Policy = "RolesPolicy")]
        [HttpGet(ApiRoutes.Identity.GetRoleById)]
        public async Task<IActionResult> GetRoleById(int id)
        {
            RoleEntity role = await authService.FindRole(r => r.Id == id);
            if (role == null)
                return NotFound();

            return Ok(role);
        }

        [Authorize(Policy = "UsersPolicy")]
        [HttpGet(ApiRoutes.Identity.GetUserById)]
        public async Task<IActionResult> GetUserById(int id)
        {
            UserEntity user = await authService.FindUser(u => u.Id == id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [Authorize(Policy = "RolesPolicy")]
        [HttpGet(ApiRoutes.Identity.GetRoles)]
        public async IAsyncEnumerable<RoleEntity> GetRoles()
        {
            var roles = authService.Roles();
            await foreach (RoleEntity role in roles)
                yield return role;
        }

        [Authorize(Policy = "UsersPolicy")]
        [HttpGet(ApiRoutes.Identity.GetUsers)]
        public async IAsyncEnumerable<UserEntity> GetUsers()
        {
            var users = authService.Users();
            await foreach (UserEntity user in users)
                yield return user;
        }

        [Authorize(Policy = "RolesPolicy")]
        [HttpGet(ApiRoutes.Identity.GetPermissionsByRole)]
        public async Task<IActionResult> GetPermissionsByRole(int idRole)
        {
            RoleEntity role = await authService.FindRole(r => r.Id == idRole);
            if (role == null)
                return BadRequest(new { Errors = new[] { "Unable to get permissions if role does not exist" } });
            var permissions = await authService.PermissionsByRole(role).ToListAsync();

            return Ok(permissions);
        }
    }
}
