using System.Threading.Tasks;
using System.Collections.Generic;

namespace Rental.Domain
{
    public interface IAuthService
    {
        Task<RoleEntity> CreateRole(RoleEntity role);
        Task<UserEntity> CreateUser(UserEntity user);
        IEnumerable<RoleEntity> GetRoles();
        IEnumerable<UserEntity> GetUsers();
    }
}
