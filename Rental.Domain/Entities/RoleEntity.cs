using System.Collections.Generic;

namespace Rental.Domain
{
    public struct DefaultRoles
    {
        public const string AdminUser = nameof(AdminUser);
        public const string CommonUser = nameof(CommonUser);
    }

    public struct Permissions
    {
        public const string canRoles = nameof(canRoles);
        public const string canUsers = nameof(canUsers);
        public const string canAircrafts = nameof(canAircrafts);
        public const string canPassengers = nameof(canPassengers);
        public const string canRentals = nameof(canRentals);
    }

    public class RoleEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<RolePermissionEntity> Permissions { get; set; }
    }

    public class PermissionEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class RolePermissionEntity
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
        public RoleEntity Role { get; set; }
        public PermissionEntity Permission { get; set; }
    }
}
