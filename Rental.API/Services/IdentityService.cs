using Microsoft.AspNetCore.Identity;
using Rental.Domain;
using Rental.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rental.API
{
    public class IdentityService : IIdentityService
    {
        readonly IAuthService authService;
        readonly IRepository<RentalContext, RoleEntity> roleRepository;
        readonly IRepository<RentalContext, UserEntity> userRepository;

        public IdentityService(
            IAuthService authService,
            IRepository<RentalContext, RoleEntity> roleRepository,
            IRepository<RentalContext, UserEntity> userRepository)
        {
            this.authService = authService;
            this.roleRepository = roleRepository;
            this.userRepository = userRepository;
        }

        public async Task<AuthenticationResult> Register(UserEntity user)
        {
            user.Role = await GetRole();
            _ = await authService.CreateUser(user);

            bool existingUser = userRepository.Exists(u => u.Username == user.Username || u.Email == user.Email);

            if (existingUser)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "User with provided email or username already exists" }
                };
            }

            return new AuthenticationResult { };
        }

        private async Task<RoleEntity> GetRole()
        {
            RoleEntity role;
            bool emptyUsers = !(await authService.Users().AnyAsync());
            (string roleName, int[] permissions) = emptyUsers ? (DefaultRoles.AdminUser, new[] { 1, 2, 3, 4, 5 }) : (DefaultRoles.CommonUser, new[] { 4, 5 });
            role = await authService.RoleByName(roleName);
            if (role == null)
                role = await authService.CreateRole(new RoleEntity { Name = roleName }, permissions);
            
            return role;
        }
    }
}
