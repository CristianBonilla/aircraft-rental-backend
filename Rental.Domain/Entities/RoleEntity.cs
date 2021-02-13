using System.Collections.Generic;

namespace Rental.Domain
{
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
        public int IdRole { get; set; }
        public int IdPermission { get; set; }
        public RoleEntity Role { get; set; }
        public PermissionEntity Permission { get; set; }
    }
}
