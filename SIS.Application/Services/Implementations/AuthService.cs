using Microsoft.AspNetCore.Identity;
using SIS.Application.Services.Interfaces;
using SIS.Domain;

namespace SIS.Application.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> RegisterAsync()
        {
            return IdentityResult.Failed(new IdentityError { Description = "Registration not implemented yet." });
        }

        public async Task<SignInResult> LoginAsync()
        {
            return SignInResult.Failed;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }

}
