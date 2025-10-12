
using EmployeeManagement.EF.Repository;
using EmployeeManagement.EF.Repository.Interface;
using EmployeeManagement.EF.TestDb;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;  
using System.Text;                     


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddDbContext<DbContextTest>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITimeLogRepository, TimeLogRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
var jwtKey = builder.Configuration["Jwt:Key"] ?? "my_secret_jwt_key_123!";
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    options.LoginPath = "/account/login";
    options.LogoutPath = "/account/accessdenied";
    options.ExpireTimeSpan = TimeSpan.FromDays(1);
});
var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
