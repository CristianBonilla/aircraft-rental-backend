using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Rental.Infrastructure;

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

        public async Task<RoleEntity> CreateRole(RoleEntity role, int[] permissionIDs)
        {
            var rolePermissions = permissionIDs.Distinct().Where(id => permissionRepository.Exists(p => p.Id == id))
                .Select(id => new RolePermissionEntity { Role = role, IdPermission = id });
            rolePermissionRepository.CreateAll(rolePermissions);
            await context.SaveAsync();

            return role;
        }

        public async Task<UserEntity> CreateUser(UserEntity user)
        {
            UserEntity userCreated = userRepository.Create(user);
            await context.SaveAsync();

            return userCreated;
        }

        public Task<RoleEntity> RoleById(int id)
        {
            RoleEntity role = roleRepository.Find(id);

            return Task.FromResult(role);
        }

        public Task<UserEntity> UserById(int id)
        {
            UserEntity user = userRepository.Find(id);

            return Task.FromResult(user);
        }

        public IEnumerable<RoleEntity> Roles() => roleRepository.Get(orderBy: o => o.OrderBy(n => n.Name));

        public IEnumerable<UserEntity> Users() => userRepository.Get(orderBy: o => o.OrderBy(u => u.FirstName).ThenBy(u => u.LastName));
    }
}
