using FinalProject.DAL;
using Microsoft.EntityFrameworkCore;
using FinalProject;
using FinalProject.Models;

var builder = WebApplication.CreateBuilder(args);

var _config = builder.Configuration;
builder.Services.Configure<GoogleRecaptchaConfig>(builder.Configuration.GetSection("GoogleRecaptcha"));
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(_config.GetConnectionString("DefaultConnection"));
});

builder.Services.BackendProjectServiceRegistration();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
          );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
