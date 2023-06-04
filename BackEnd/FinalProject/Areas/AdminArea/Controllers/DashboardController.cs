using FinalProject.Areas.AdminArea.ViewModels;
using FinalProject.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
        [Authorize(Roles ="SuperAdmin, Admin")]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public DashboardController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task <IActionResult> Index()
        {
            DashboardVM dashboardVM = new()
            {
                Artists = await _appDbContext.Artists.ToListAsync(),
                Genres = await _appDbContext.Genres.ToListAsync(),
                Albums = await _appDbContext.Albums.ToListAsync(),
                Songs  = await _appDbContext.Songs.ToListAsync(),
                
            };

            return View(dashboardVM);
        }
    }
}
