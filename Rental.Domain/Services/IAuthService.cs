using System;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace Rental.Domain
{
    public interface IAuthService
    {
        Task<RoleEntity> CreateRole(RoleEntity role, Guid[] permissionIDs);
        Task<UserEntity> CreateUser(UserEntity user);
        Task<RoleEntity> FindRole(Expression<Func<RoleEntity, bool>> expression);
        Task<UserEntity> FindUser(Expression<Func<UserEntity, bool>> expression);
        Task<bool> RoleExists(Expression<Func<RoleEntity, bool>> expression);
        Task<bool> UserExists(Expression<Func<UserEntity, bool>> expression);
        IAsyncEnumerable<PermissionEntity> PermissionsByRole(RoleEntity role);
        IAsyncEnumerable<RoleEntity> Roles();
        IAsyncEnumerable<UserEntity> Users();
    }
}
