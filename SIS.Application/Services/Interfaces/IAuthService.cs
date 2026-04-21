using Microsoft.AspNetCore.Identity;

namespace SIS.Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterAsync();
        Task<SignInResult> LoginAsync();
        Task LogoutAsync();
    }
}
