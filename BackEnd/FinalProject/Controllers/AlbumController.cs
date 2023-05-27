using FinalProject.DAL;
using FinalProject.Models;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace FinalProject.Controllers
{
    public class AlbumController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public AlbumController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var query = _appDbContext.Albums.Include(a => a.Artist).Include(g => g.Genre).Include(s => s.Songs);

            ViewBag.AlbumCount = query.Count();
            AlbumVM albumVM = new()
            {
                Genres = await _appDbContext.Genres.Where(g => !g.IsDeleted).ToListAsync(),
                Albums = await query.ToListAsync(),
            };

            return View(albumVM);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return NotFound();
            Album? checkAlbum = await _appDbContext.Albums.FirstOrDefaultAsync(a => a.Id == id);
            if (checkAlbum==null)
            {
                return NotFound();
            }
            AlbumVM albumVM = new()
            {
                Genres = await _appDbContext.Genres.Where(g => !g.IsDeleted).ToListAsync(),
                Album = await _appDbContext.Albums.Where(a => !a.IsDeleted).Include(a => a.Artist).Include(a => a.Genre).FirstOrDefaultAsync(),
                Songs = await _appDbContext.Songs.Where(a => a.AlbumId == id).ToListAsync()
                
        };

           return View(albumVM);
        }
    }
}
