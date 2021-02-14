using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Rental.Domain;
using Rental.Infrastructure;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Rental.API
{
    public class IdentityService : IIdentityService
    {
        readonly IAuthService authService;
        readonly JwtSettings jwtSettings;

        public IdentityService(IAuthService authService, IOptions<JwtSettings> options) =>
            (this.authService, jwtSettings) = (authService, options.Value);

        public async Task<AuthenticationResult> Register(UserEntity user, RoleRequest roleRequest = null)
        {
            bool existingUser = await authService.UserExists(user.Username, user.Email);
            if (existingUser)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "User with provided email or username already exists" }
                };
            }
            RoleEntity role = await GetRegistrationRole(roleRequest);
            if (role == null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "The user cannot be created if an existing role is not specified" }
                };
            }
            user.Role = role;
            UserEntity userCreated = await authService.CreateUser(user);
            var permissions = await authService.PermissionsByRole(userCreated.Role)
                .Select(p => p.Name)
                .ToListAsync();
            var permissionsJson = JsonConvert.SerializeObject(permissions, Formatting.Indented);
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(jwtSettings.Secret);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, userCreated.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, userCreated.Email),
                    new Claim(JwtRegisteredClaimNames.NameId, userCreated.Id.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, userCreated.Username),
                    new Claim(ClaimTypes.Role, userCreated.Role.Name),
                    new Claim("permissions", permissionsJson, JsonClaimValueTypes.JsonArray)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return new AuthenticationResult
            {
                Success = true,
                Token = tokenHandler.WriteToken(token)
            };
        }

        private async Task<RoleEntity> GetRegistrationRole(RoleRequest roleRequest)
        {
            bool emptyUsers = !(await authService.Users().AnyAsync());
            if (!emptyUsers && roleRequest != null)
                return await authService.RoleByName(roleRequest.Name);
            (string roleName, int[] permissions) = emptyUsers ? (DefaultRoles.AdminUser, new[] { 1, 2, 3, 4, 5 }) : (DefaultRoles.CommonUser, new[] { 4, 5 });
            RoleEntity role = await authService.RoleByName(roleName);
            if (role == null)
                role = await authService.CreateRole(new RoleEntity { Name = roleName }, permissions);

            return role;
        }
    }
}
