using FinalProject.DAL;
using FinalProject.Models;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace FinalProject.Controllers
{
    public class AlbumController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<AppUser> _userManager;
        public AlbumController(AppDbContext appDbContext, UserManager<AppUser> userManager)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
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
#pragma warning disable CS8601 // Возможно, назначение-ссылка, допускающее значение NULL.
            AlbumVM albumVM = new()
            {
                Genres = await _appDbContext.Genres.Where(g => !g.IsDeleted).ToListAsync(),
                Album  = await _appDbContext.Albums.Where(a => !a.IsDeleted).Include(a => a.Artist).Include(bd => bd.Comments).ThenInclude(u=>u.User).Include(a => a.Genre).FirstOrDefaultAsync(a => a.Id == id),
                Songs = await _appDbContext.Songs.Where(a => a.AlbumId == id).ToListAsync()
                
        };
#pragma warning restore CS8601 // Возможно, назначение-ссылка, допускающее значение NULL.

            return View(albumVM);
        }
        [HttpPost]
        public async Task<IActionResult> AddComment(string Content, int albumId)
        {
            AppUser? user;

            if (User.Identity.IsAuthenticated)
            {
                user = await _userManager.FindByNameAsync(User.Identity.Name);

            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            Comment comment = new()
            {
                CreatedTime = DateTime.Now,
                AppUserId = user.Id,
                AlbumId = albumId,
                Content = Content
            };
            _appDbContext.Comments.Add(comment);
            _appDbContext.SaveChanges();
            return RedirectToAction("Detail", new { id = albumId });
        }
        public async Task<IActionResult> DeleteComment(int id)
        {
            Comment? comment = await _appDbContext.Comments.FirstOrDefaultAsync(b => b.Id == id);
            _appDbContext.Comments.Remove(comment);
            _appDbContext.SaveChanges();
            return RedirectToAction("Detail", new { id = comment.AlbumId });
        }

    }
}
