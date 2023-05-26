using FinalProject.DAL;
using FinalProject.Models;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    public class AlbumController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public AlbumController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task <IActionResult> Index()
        {
            var query =  _appDbContext.Albums.Include(a => a.Artist).Include(g=>g.Genre).Include(s => s.Songs);

            ViewBag.AlbumCount = query.Count();
            AlbumVM albumVM = new()
            {
              Genres = await _appDbContext.Genres.Where(g=>!g.IsDeleted).ToListAsync(),
              Albums  = await query.ToListAsync(),
        };
            
       

            return View(albumVM);
        }
    }
}
