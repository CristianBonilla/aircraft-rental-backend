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
        public const string CanActivateRoles = nameof(CanActivateRoles);
        public const string CanActivateUsers = nameof(CanActivateUsers);
        public const string CanActivateAircrafts = nameof(CanActivateAircrafts);
        public const string CanActivatePassengers = nameof(CanActivatePassengers);
        public const string CanActivateRentals = nameof(CanActivateRentals);
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
