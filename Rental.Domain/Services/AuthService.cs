using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Rental.Infrastructure;
using System.Linq.Expressions;
using System;

namespace Rental.Domain
{
    public class AuthService : IAuthService
    {
        readonly IRepositoryContext<RentalContext> context;
        readonly IRepository<RentalContext, RoleEntity> roleRepository;
        readonly IRepository<RentalContext, UserEntity> userRepository;
        readonly IRepository<RentalContext, PermissionEntity> permissionRepository;
        readonly IRepository<RentalContext, RolePermissionEntity> rolePermissionRepository;

        public AuthService(
            IRepositoryContext<RentalContext> context,
            IRepository<RentalContext, RoleEntity> roleRepository,
            IRepository<RentalContext, UserEntity> userRepository,
            IRepository<RentalContext, PermissionEntity> permissionRepository,
            IRepository<RentalContext, RolePermissionEntity> rolePermissionRepository)
        {
            this.context = context;
            this.roleRepository = roleRepository;
            this.userRepository = userRepository;
            this.permissionRepository = permissionRepository;
            this.rolePermissionRepository = rolePermissionRepository;
        }

        public async Task<RoleEntity> CreateRole(RoleEntity role, Guid[] permissionIDs)
        {
            var rolePermissions = permissionIDs.Distinct().Where(id => permissionRepository.Exists(p => p.Id == id))
                .Select(id => new RolePermissionEntity { Role = role, PermissionId = id });
            rolePermissionRepository.CreateAll(rolePermissions);
            _ = await context.SaveAsync();

            return role;
        }

        public async Task<UserEntity> CreateUser(UserEntity user)
        {
            UserEntity userCreated = userRepository.Create(user);
            _ = await context.SaveAsync();

            return userCreated;
        }

        public Task<RoleEntity> FindRole(Expression<Func<RoleEntity, bool>> expression)
        {
            RoleEntity roleFound = roleRepository.Find(expression);

            return Task.FromResult(roleFound);
        }

        public Task<UserEntity> FindUser(Expression<Func<UserEntity, bool>> expression)
        {
            UserEntity userFound = userRepository.Find(expression);

            return Task.FromResult(userFound);
        }

        public Task<bool> RoleExists(Expression<Func<RoleEntity, bool>> expression)
        {
            bool existingRole = roleRepository.Exists(expression);

            return Task.FromResult(existingRole);
        }

        public Task<bool> UserExists(Expression<Func<UserEntity, bool>> expression)
        {
            bool existingUser = userRepository.Exists(expression);

            return Task.FromResult(existingUser);
        }

        public IAsyncEnumerable<PermissionEntity> PermissionsByRole(RoleEntity role)
        {
            var permissions = rolePermissionRepository.Get(p => p.RoleId == role.Id, null, i => i.Permission)
                .Select(p => permissionRepository.Find(p.PermissionId))
                .OrderBy(o => o.Id)
                .ToAsyncEnumerable();

            return permissions;
        }

        public IAsyncEnumerable<RoleEntity> Roles()
        {
            var roles = roleRepository.Get(null, o => o.OrderBy(n => n.Name), r => r.Permissions).ToAsyncEnumerable();

            return roles;
        }

        public IAsyncEnumerable<UserEntity> Users()
        {
            var users = userRepository.Get(orderBy: o => o.OrderBy(u => u.RoleId).ThenBy(u => u.FirstName)).ToAsyncEnumerable();

            return users;
        }
    }
}
