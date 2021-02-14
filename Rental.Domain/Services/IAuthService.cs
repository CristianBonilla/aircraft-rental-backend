using System.Threading.Tasks;
using System.Collections.Generic;

namespace Rental.Domain
{
    public interface IAuthService
    {
        Task<RoleEntity> CreateRole(RoleEntity role, int[] permissionIDs);
        Task<UserEntity> CreateUser(UserEntity user);
        Task<RoleEntity> RoleById(int id);
        Task<RoleEntity> RoleByName(string name);
        Task<UserEntity> UserById(int id);
        IAsyncEnumerable<RoleEntity> Roles();
        IAsyncEnumerable<UserEntity> Users();
    }
}
