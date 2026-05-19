using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using SIS.Application.Services.Interfaces;
using SIS.Domain;
using LoginRequest = SIS.Application.DTOs.Auth.LoginRequest;
using RegisterRequest = SIS.Application.DTOs.Auth.RegisterRequest;

namespace SIS.Infrastructure.Services
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

        public async Task<IdentityResult> RegisterAsync(RegisterRequest request)
        {
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                // FullName = request.FullName // Assuming you added this to ApplicationUser
            };

            return await _userManager.CreateAsync(user, request.Password);
        }

        public async Task<SignInResult> LoginAsync(LoginRequest request)
        {
            // For APIs using Cookies: PasswordSignInAsync handles everything
            // For APIs using JWT: You would use CheckPasswordAsync here instead
            return await _signInManager.PasswordSignInAsync(
                request.Email,
                request.Password,
                request.RememberMe,
                lockoutOnFailure: false);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}