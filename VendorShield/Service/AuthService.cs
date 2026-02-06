using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity.Data;
using VendorShield.Database;
using VendorShield.Model;
using VendorShield.Utility;

namespace VendorShield.Service
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<bool> RegisterAsync(Model.RegisterRequest Request)
        {
            if (_context.AdminUsers.Any(u => u.Email == Request.Email))
            {
                return false; // Email already exists
            }
            var user = new AdminUser
            {
                FirstName = Request.FirstName,
                LastName = Request.LastName,
                Email = Request.Email,
                PasswordHash = Utility.PasswordHasher.HashPassword(Request.Password)
            };
            await _context.AdminUsers.AddAsync(user);
            await _context.SaveChangesAsync();
            return true;

        }
        public async Task<bool> LoginAsync(Model.LoginRequest Request)
        {
            var user = _context.AdminUsers.FirstOrDefault(u => u.Email == Request.Email);
            if (user == null || !Utility.PasswordHasher.VerifyPassword(Request.Password, user.PasswordHash))

            {
                return false;
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("UserId", user.Id.ToString())
            };
            var claimidentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authproperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
            };
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
                throw new Exception("No HttpContext available. Make sure IHttpContextAccessor is registered.");
            await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimidentity), authproperties);
            return true;





        }
        public async Task LogoutAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
                throw new Exception("No HttpContext available. Make sure IHttpContextAccessor is registered.");
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
