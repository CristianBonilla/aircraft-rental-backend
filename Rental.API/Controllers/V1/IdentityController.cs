using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rental.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rental.API.Controllers.V1
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class IdentityController : ControllerBase
    {
        readonly IAuthService authService;
        readonly IIdentityService identityService;

        public IdentityController(IAuthService authService, IIdentityService identityService) =>
            (this.authService, this.identityService) = (authService, identityService);

        //[Authorize(Policy = "")]
        [HttpPost(ApiRoutes.Identity.CreateRole)]
        public async Task<IActionResult> CreateRole([FromBody] RoleRequest roleRequest)
        {
            bool exstingRole = await authService.RoleExists(roleRequest.Name);
            if (exstingRole)
                return BadRequest(new { Errors = new[] { "The role is already registered" } });
            RoleEntity role = await authService.CreateRole(new RoleEntity { Name = roleRequest.Name }, roleRequest.PermissionsIDs);

            return Ok(role);
        }

        [AllowAnonymous]
        [HttpPost(ApiRoutes.Identity.Register)]
        public async Task<IActionResult> Register([FromBody] UserEntity user)
        {
            var authResponse = await identityService.Register(user);
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
