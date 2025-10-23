using EmployeeManagement.EF.Repository;
using EmployeeManagement.EF.Repository.Interface;
using EmployeeManagement.EF.TestDb;
using EmployeeManagement.Service;
using EmployeeManagement.Services;
using Hangfire;
using Hangfire.MySql;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add MVC
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

// Add DbContext
builder.Services.AddDbContext<DbContextTest>();

// Cookie Auth
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/account/login";
        options.AccessDeniedPath = "/account/accessdenied";
        options.ExpireTimeSpan = TimeSpan.FromDays(1);
        options.SlidingExpiration = true;
    });

// Authorization
builder.Services.AddAuthorization();

// Repository + Services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<ITimeLogRepository, TimeLogRepository>();
builder.Services.AddScoped<IEmailservice, EmailService>();
builder.Services.AddScoped<ILateTimeService, LateTimeService>();

//  Hangfire (MySQL)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddHangfire(config =>
{
    config
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings();
});

builder.Services.AddHangfireServer();

var app = builder.Build();

// Middlewares
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

GlobalConfiguration.Configuration
    .UseStorage(new MySqlStorage(connectionString, new MySqlStorageOptions
    {
        PrepareSchemaIfNecessary = true
    }));

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

//  Hangfire Dashboard
app.UseHangfireDashboard("/hangfire");

//  Job mỗi ngày 7h sáng
RecurringJob.AddOrUpdate<ILateTimeService>(
    "daily-report",
    service => service.SendYesterdayTimeLogReportAsync(),
    "0 7 * * *", // 7h sáng
    new RecurringJobOptions
    {
        TimeZone = TimeZoneInfo.Local
    }
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
