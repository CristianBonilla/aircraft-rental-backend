using System;

namespace Rental.API
{
    public class RoleRequest
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public Guid[] PermissionsIDs { get; set; }
    }
}
