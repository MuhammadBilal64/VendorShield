using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using VendorShield.Database;
using VendorShield.Model;
using VendorShield.Utility;
using RegisterRequest = VendorShield.Model.RegisterRequest;

namespace VendorShield.Service
{
    public class AuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<bool> RegisterAsync(RegisterRequest Request)
        {
            var user = await _userManager.FindByEmailAsync(Request.Email);
            if (user != null)
            {
                return false;
            }
            var UserToCreate = new ApplicationUser
            {
                UserName = Request.Email,
                Email = Request.Email,
                FirstName = Request.FirstName,
                LastName = Request.LastName,
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            };
            var result = await _userManager.CreateAsync(UserToCreate, Request.Password);
            return result.Succeeded;



        }
        public async Task<bool> LoginAsync(Model.LoginRequest Request)
        {
            var result = await _signInManager.PasswordSignInAsync(Request.Email, Request.Password, isPersistent: true, lockoutOnFailure: true);
            return result.Succeeded;
        }
        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();

        }
    }
}
