using System.Linq;
using System.Threading.Tasks;
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
            

        //[Authorize(Policy = "")]
        [HttpPost(ApiRoutes.Identity.CreateRole)]
        public async Task<IActionResult> CreateRole([FromBody] RoleRequest roleRequest)
        {
            bool exstingRole = await authService.RoleExists(r => r.Name == roleRequest.Name);
            if (exstingRole)
                return BadRequest(new { Errors = new[] { "The role is already registered" } });
            if (!roleRequest.PermissionsIDs.Any())
                return BadRequest(new { Errors = new[] { "The permissions to grant the role were not defined" } });
            RoleEntity role = await authService.CreateRole(new RoleEntity { Name = roleRequest.Name }, roleRequest.PermissionsIDs);

            return Ok(role);
        }

        [AllowAnonymous]
        [HttpPost(ApiRoutes.Identity.Register)]
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

            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token
            });
        }

        [AllowAnonymous]
        [HttpPost(ApiRoutes.Identity.Login)]
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

            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token
            });
        }
    }
}