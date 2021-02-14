using Rental.Domain;

namespace Rental.API
{
    public class RoleRequest
    {
        public string Name { get; set; }
        public int[] PermissionsIDs { get; set; }
    }
}
