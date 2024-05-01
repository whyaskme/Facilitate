using Facilitate.Admin.Components;
using Facilitate.Admin.Components.Account;
using Facilitate.Admin.Data;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ServiceStack;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDevExpressBlazor(options => {
    options.BootstrapVersion = DevExpress.Blazor.BootstrapVersion.v5;
    options.SizeMode = DevExpress.Blazor.SizeMode.Medium;
});

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddSingleton<WebServices>();

builder.Services.AddCors(x => x.AddPolicy("externalRequests",
                    policy => policy
                .WithOrigins("https://jsonip.com")));

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var requireConfirmedAccount = false;
var requireConfirmedEmail = false;
var requireConfirmedPhoneNumber = false;

builder.Services.AddIdentityCore<ApplicationUser>(options =>
        {
            options.SignIn.RequireConfirmedAccount = requireConfirmedAccount;
            options.SignIn.RequireConfirmedEmail = requireConfirmedEmail;
            options.SignIn.RequireConfirmedPhoneNumber = requireConfirmedPhoneNumber;
        }
    )
    .AddRoles<IdentityRole>()
    .AddRoleManager<RoleManager<IdentityRole>>()
    .AddUserManager<UserManager<ApplicationUser>>()
    .AddRoleStore<RoleStore<IdentityRole, ApplicationDbContext>>()
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

builder.Services.AddScoped(sp =>
    new HttpClient
    {
        BaseAddress = new Uri(builder.Configuration["FrontendUrl"] ?? "https://localhost:8080")
    });

builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

IEnumerable<IdentityError>? identityErrors;

try
{
    using (var scope = app.Services.CreateScope())
    {
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var roles = new[] { "System Admin", "Site Admin", "Group Admin", "Project Manager", "Vendor", "Member" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                IdentityRole roleRole = new IdentityRole(role);
                await roleManager.CreateAsync(roleRole);
            }
        }
    }

    using (var scope = app.Services.CreateScope())
    {
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        ApplicationUser adminUser = new ApplicationUser();
        adminUser.Email = "admin@facilitate.org";
        adminUser.UserName = adminUser.Email;

        var adminPwd = "!Facilitate2024#";

        // Check if this admin already exists
        var existingAdmin = await userManager.FindByEmailAsync(adminUser.Email);
        if(existingAdmin == null)
        {
            var result = await userManager.CreateAsync(adminUser, adminPwd);
            if (!result.Succeeded)
            {
                identityErrors = result.Errors;
                return;
            }

            // Add user to roles
            await userManager.AddToRoleAsync(adminUser, "System Admin");
            await userManager.AddToRoleAsync(adminUser, "Site Admin");
            await userManager.AddToRoleAsync(adminUser, "Group Admin");
            await userManager.AddToRoleAsync(adminUser, "Project Manager");
            await userManager.AddToRoleAsync(adminUser, "Vendor");
            await userManager.AddToRoleAsync(adminUser, "Member");
        }
    }
}
catch(Exception ex)
{
    Console.WriteLine(ex.Message);
}

app.Run();
