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
    public class IdentityController : ControllerBase
    {
        readonly IIdentityService identityService;

        public IdentityController(IIdentityService identityService) =>
            (this.identityService) = (identityService);

        [HttpPost(ApiRoutes.Identity.Register)]
        public async Task<IActionResult> Register([FromBody] UserEntity user)
        {
            var authResponse = await identityService.Register(user);

            return Ok();
        }
    }
}
