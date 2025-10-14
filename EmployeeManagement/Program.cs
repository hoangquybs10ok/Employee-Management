using EmployeeManagement.EF.Repository;
using EmployeeManagement.EF.Repository.Interface;
using EmployeeManagement.EF.TestDb;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

//  Add services
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddDbContext<DbContextTest>();

// Authentication bằng Cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/account/login";                // Khi chưa đăng nhập
        options.AccessDeniedPath = "/account/accessdenied"; // Khi bị cấm truy cập
        options.ExpireTimeSpan = TimeSpan.FromDays(1);     // Cookie tồn tại 1 ngày
        options.SlidingExpiration = true;                 // Tự động làm mới thời gian
    });

//  Authorization
builder.Services.AddAuthorization();

//  Đăng ký Repository
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<ITimeLogRepository, TimeLogRepository>();

var app = builder.Build();

// Middlewares
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
