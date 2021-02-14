using System.Threading.Tasks;
using System.Collections.Generic;

namespace Rental.Domain
{
    public interface IAuthService
    {
        Task<RoleEntity> CreateRole(RoleEntity role, int[] permissionIDs);
        Task<UserEntity> CreateUser(UserEntity user);
        Task<RoleEntity> RoleById(int id);
        Task<UserEntity> UserById(int id);
        IEnumerable<RoleEntity> Roles();
        IEnumerable<UserEntity> Users();
    }
}
