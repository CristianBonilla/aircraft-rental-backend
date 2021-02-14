using Rental.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rental.API
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> Register(UserEntity user, RoleRequest roleRequest = null);
    }
}
