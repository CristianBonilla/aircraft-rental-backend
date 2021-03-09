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
        [HttpPost(ApiRoutes.V1.Identity.Register)]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest userRegisterRequest)
        {
            UserEntity user = mapper.Map<UserEntity>(userRegisterRequest);
            AuthenticationResult authResponse = await identityService.Register(user, userRegisterRequest.Role);
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
        [HttpPost(ApiRoutes.V1.Identity.Login)]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest userLoginRequest)
        {
            AuthenticationResult authResponse = await identityService.Login(userLoginRequest);
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

        [HttpPost(ApiRoutes.V1.Identity.UserExists)]
        public async Task<IActionResult> UserExists([FromBody] UserResponse userResponse)
        {
            UserEntity user = mapper.Map<UserEntity>(userResponse);
            bool existingUser = await identityService.UserExists(user);

            return Ok(existingUser);
        }

        [Authorize(Policy = "RolesPolicy")]
        [HttpPost(ApiRoutes.V1.Identity.CreateRole)]
        public async Task<IActionResult> CreateRole([FromBody] RoleRequest roleRequest)
        {
            bool exstingRole = await authService.RoleExists(r => r.Name == roleRequest.Name);
            if (exstingRole)
                return BadRequest(new { Errors = new[] { "The role is already registered" } });
            if (!roleRequest.PermissionsIDs.Any())
                return BadRequest(new { Errors = new[] { "The permissions to grant the role were not defined" } });
            RoleEntity role = mapper.Map<RoleEntity>(roleRequest);
            RoleEntity roleCreated = await authService.CreateRole(role, roleRequest.PermissionsIDs);
            RoleResponse roleResponse = mapper.Map<RoleResponse>(roleCreated);

            return Ok(roleResponse);
        }

        [Authorize(Policy = "UsersPolicy")]
        [HttpPost(ApiRoutes.V1.Identity.CreateUser)]
        public async Task<IActionResult> CreateUser([FromBody] UserRegisterRequest userRegisterRequest)
        {
            RoleEntity role = await authService.FindRole(r => r.Name == userRegisterRequest.Role);
            if (role == null)
                return BadRequest(new { Errors = new[] { "The user cannot be created if an existing role is not specified" } });
            UserEntity user = mapper.Map<UserEntity>(userRegisterRequest);
            user.RoleId = role.Id;
            bool existingUser = await identityService.UserExists(user);
            if (existingUser)
                return BadRequest(new { Errors = new[] { "User with provided email or username already exists" } });
            UserEntity userCreated = await authService.CreateUser(user);
            UserResponse userResponse = mapper.Map<UserResponse>(userCreated);

            return Ok(userResponse);
        }

        [Authorize(Policy = "RolesPolicy")]
        [HttpGet(ApiRoutes.V1.Identity.GetRoleById)]
        public async Task<IActionResult> GetRoleById(int id)
        {
            RoleEntity role = await authService.FindRole(r => r.Id == id);
            if (role == null)
                return NotFound();
            RoleResponse roleResponse = mapper.Map<RoleResponse>(role);

            return Ok(role);
        }

        [Authorize(Policy = "UsersPolicy")]
        [HttpGet(ApiRoutes.V1.Identity.GetUserById)]
        public async Task<IActionResult> GetUserById(int id)
        {
            UserEntity user = await authService.FindUser(u => u.Id == id);
            if (user == null)
                return NotFound();
            UserResponse userResponse = mapper.Map<UserResponse>(user);

            return Ok(user);
        }

        [HttpGet(ApiRoutes.V1.Identity.GetRoles)]
        public async IAsyncEnumerable<RoleResponse> GetRoles()
        {
            var roles = authService.Roles();
            await foreach (RoleEntity role in roles)
                yield return mapper.Map<RoleResponse>(role);
        }

        [Authorize(Policy = "UsersPolicy")]
        [HttpGet(ApiRoutes.V1.Identity.GetUsers)]
        public async IAsyncEnumerable<UserResponse> GetUsers()
        {
            var users = authService.Users();
            await foreach (UserEntity user in users)
                yield return mapper.Map<UserResponse>(user);
        }

        [Authorize(Policy = "RolesPolicy")]
        [HttpGet(ApiRoutes.V1.Identity.GetPermissionsByRole)]
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
