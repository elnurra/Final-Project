using FinalProject.DAL;
using FinalProject.Models;
using FinalProject.Services.Interfaces;
using FinalProject.Services;
using Microsoft.AspNetCore.Identity;

namespace FinalProject
{
    public static class ServiceRegistration
    {
        public static void BackendProjectServiceRegistration(this IServiceCollection services)
        {
            // Add services to the container.
            services.AddControllersWithViews();
            //------------------------------------------------------------------------------------------------
            services.AddControllersWithViews().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
             );
            //------------------------------------------------------------------------------------------------
            services.AddHttpContextAccessor();
            //------------------------------------------------------------------------------------------------
            services.AddScoped<IEmailService, EmailService>();
            //------------------------------------------------------------------------------------------------
            services.AddScoped<IFileService, FileService>();
            //------------------------------------------------------------------------------------------------
            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.User.RequireUniqueEmail = true;
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(50);
                options.Lockout.MaxFailedAccessAttempts = 3;
            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
            //------------------------------------------------------------------------------------------------
            services.AddScoped<GoogleCaptchaService>();
        }
    }
}
