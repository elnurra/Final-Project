using FinalProject.Areas.AdminArea.ViewModels;
using FinalProject.DAL;
using FinalProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Areas.AdminArea.Controllers
{
        [Area("AdminArea")]
        [Authorize(Roles ="SuperAdmin, Admin")]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DashboardController(AppDbContext appDbContext, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task <IActionResult> Index()
        {
            DashboardVM dashboardVM = new()
            {
                Artists = await _appDbContext.Artists.ToListAsync(),
                Genres = await _appDbContext.Genres.ToListAsync(),
                Albums = await _appDbContext.Albums.ToListAsync(),
                Songs = await _appDbContext.Songs.ToListAsync(),
                Sliders = await _appDbContext.Sliders.ToListAsync(),
                Events = await _appDbContext.Events.ToListAsync(),
                AppUsers = await _appDbContext.Users.ToListAsync(),
                IdentityRoles = await _roleManager.Roles.ToListAsync(),
                
            };

            return View(dashboardVM);
        }
    }
}
