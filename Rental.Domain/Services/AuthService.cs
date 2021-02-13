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

        public AuthService(
            IRepositoryContext<RentalContext> context,
            IRepository<RentalContext, RoleEntity> roleRepository,
            IRepository<RentalContext, UserEntity> userRepository)
        {
            this.context = context;
            this.roleRepository = roleRepository;
            this.userRepository = userRepository;
        }

        public async Task<RoleEntity> CreateRole(RoleEntity role)
        {
            RoleEntity roleCreated = roleRepository.Create(role);
            await context.SaveAsync();

            return roleCreated;
        }

        public async Task<UserEntity> CreateUser(UserEntity user)
        {
            UserEntity userCreated = userRepository.Create(user);
            await context.SaveAsync();

            return userCreated;
        }

        public IEnumerable<RoleEntity> GetRoles() => roleRepository.Get(orderBy: o => o.OrderBy(n => n.RoleName));

        public IEnumerable<UserEntity> GetUsers() => userRepository.Get(orderBy: o => o.OrderBy(u => u.FirstName).ThenBy(u => u.LastName));
    }
}
