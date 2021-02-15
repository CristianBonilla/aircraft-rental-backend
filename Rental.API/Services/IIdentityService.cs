using System.Threading.Tasks;
using Rental.Domain;

namespace Rental.API
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> Register(UserEntity user, string roleName = null);
        Task<AuthenticationResult> Login(UserLoginRequest userLoginRequest);
    }
}
