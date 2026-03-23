using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VendorShield.Components;
using VendorShield.DAL;
using VendorShield.Database;
using VendorShield.IDAL;
using VendorShield.IService;
using VendorShield.Model;
using VendorShield.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<IVendorDAL, VendorDAL>();
builder.Services.AddScoped<IVendorService, VendorService>();
builder.Services.AddScoped<IPurchaseOrderDAL, PurchaseOrderDAL>();
builder.Services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
builder.Services.AddScoped<IDeliveryDAL, DeliveryDAL>();
builder.Services.AddScoped<IDeliveryService, DeliveryService>();
builder.Services.AddScoped<IIncidentDAL, IncidentDAL>();
builder.Services.AddScoped<IIncidentService, IncidentService>();
builder.Services.AddScoped<IScoringDAL, ScoringDAL>();
builder.Services.AddScoped<IScoringService, ScoringService>();
builder.Services.AddScoped<IScoringConfigDAL, ScoringConfigDAL>();
builder.Services.AddScoped<IScoringConfigService, ScoringConfigService>();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    options.User.RequireUniqueEmail = true;


}).AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();


builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/auth/login";
    options.AccessDeniedPath = "/pending-access";
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.SlidingExpiration = true;
});


builder.Services.AddAuthorization();

// Required for Blazor authorization components (AuthorizeView, [Authorize] routing)
// to be able to resolve AuthenticationStateProvider via CascadingAuthenticationState.
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddHttpClient();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("VendorShieldContext")),
    ServiceLifetime.Scoped); // 
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

await SeedRolesAndAdminAsync(app);

app.UseAuthentication();
app.UseAuthorization();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

// NOTE:
// Blazor Server interactive components run over SignalR, so writing auth cookies
// during a component event can fail with "Headers are read-only, response has already started".
// This endpoint performs the Identity sign-in via a normal HTTP request so Set-Cookie works.
app.MapPost("/auth/api/login", async (
        LoginRequest request,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager) =>
    {
        var result = await signInManager.PasswordSignInAsync(
            request.Email,
            request.Password,
            isPersistent: true,
            lockoutOnFailure: true);

        if (!result.Succeeded)
        {
            return Results.Json(new { message = "Invalid email or password." }, statusCode: 401);
        }

        var user = await userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return Results.Ok(new { redirectUrl = "/pending-access" });
        }

        var roles = await userManager.GetRolesAsync(user);
        var hasAnyRole =
            roles.Contains("Admin") ||
            roles.Contains("Buyer") ||
            roles.Contains("Viewer");

        return Results.Ok(new { redirectUrl = hasAnyRole ? "/" : "/pending-access" });
    })
    .DisableAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

static async Task SeedRolesAndAdminAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var roles = new[] { "Admin", "Buyer", "Viewer" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}
