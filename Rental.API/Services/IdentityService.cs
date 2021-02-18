using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Rental.Domain;

namespace Rental.API
{
    public class IdentityService : IIdentityService
    {
        readonly IAuthService authService;
        readonly JwtSettings jwtSettings;

        public IdentityService(IAuthService authService, IOptions<JwtSettings> options) =>
            (this.authService, jwtSettings) = (authService, options.Value);

        public async Task<AuthenticationResult> Register(UserEntity user, string roleName = null)
        {
            bool existingUser = await authService.UserExists(u => u.Username == user.Username || u.Email == user.Email);
            if (existingUser)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "User with provided email or username already exists" }
                };
            }
            RoleEntity role = await GetRole(roleName);
            if (role == null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "The user cannot be created if an existing role is not specified" }
                };
            }
            user.Role = role;
            UserEntity userCreated = await authService.CreateUser(user);
            string permissionsJson = await GetPermissionsByRole(role);

            return GenerateAuthenticationForUser(userCreated, permissionsJson);
        }

        public async Task<AuthenticationResult> Login(UserLoginRequest userLoginRequest)
        {
            UserEntity user = userLoginRequest.Type switch
            {
                UserLoginType.Username => await authService.FindUser(u => u.Username == userLoginRequest.UsernameOrEmail),
                _ => await authService.FindUser(u => u.Email == userLoginRequest.UsernameOrEmail)
            };
            if (user == null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "User is not registered or is incorrect" }
                };
            }
            var userValidPassword = user.Password == userLoginRequest.Password;
            if (!userValidPassword)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "User password is invalid" }
                };
            }
            RoleEntity role = await authService.FindRole(r => r.Id == user.RoleId);
            string permissionsJson = await GetPermissionsByRole(role);

            return GenerateAuthenticationForUser(user, permissionsJson);
        }

        private AuthenticationResult GenerateAuthenticationForUser(UserEntity user, string permissionsJson)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(jwtSettings.Secret);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Username),
                    new Claim(ClaimTypes.Role, user.Role.Name),
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

        private async Task<string> GetPermissionsByRole(RoleEntity role)
        {
            var permissions = await authService.PermissionsByRole(role)
                .Select(p => p.Name)
                .ToListAsync();
            var permissionsJson = JsonConvert.SerializeObject(permissions, Formatting.Indented);

            return permissionsJson;
        }

        private async Task<RoleEntity> GetRole(string roleName = null)
        {
            bool emptyUsers = !(await authService.Users().AnyAsync());
            if (!emptyUsers && roleName != null)
                return await authService.FindRole(r => r.Name == roleName);
            string defaultRoleName = emptyUsers ? DefaultRoles.AdminUser : DefaultRoles.CommonUser;

            return await authService.FindRole(r => r.Name == defaultRoleName);
        }
    }
}
