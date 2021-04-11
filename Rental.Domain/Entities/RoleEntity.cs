using System;
using System.Collections.Generic;

namespace Rental.Domain
{
    public struct DefaultRoles
    {
        public const string AdminUser = nameof(AdminUser);
        public const string CommonUser = nameof(CommonUser);
        public const string PassengerUser = nameof(PassengerUser);
        public const string PilotUser = nameof(PilotUser);
    }

    public struct Permissions
    {
        public const string CanRoles = nameof(CanRoles);
        public const string CanUsers = nameof(CanUsers);
        public const string CanRentals = nameof(CanRentals);
        public const string CanAircrafts = nameof(CanAircrafts);
        public const string CanPassengers = nameof(CanPassengers);
    }

    public class RoleEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public ICollection<RolePermissionEntity> Permissions { get; set; }
    }

    public class PermissionEntity
    {
        public Guid Id { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }

    public class RolePermissionEntity
    {
        public Guid RoleId { get; set; }
        public Guid PermissionId { get; set; }
        public RoleEntity Role { get; set; }
        public PermissionEntity Permission { get; set; }
    }
}
