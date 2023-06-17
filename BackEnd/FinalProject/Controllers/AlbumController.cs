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

        public async Task<IActionResult> Index(int page = 1, int take = 4)
        {
            var query = _appDbContext.Albums.Include(a => a.Artist).Include(g => g.Genre).Include(s => s.Songs);
            List<Album> albums = await query.Where(a => !a.IsDeleted).ToListAsync();

            ViewBag.AlbumCount = query.Count();
            int pageCount = CalculatePageCount(albums, take);

            AlbumVM albumVM = new()
            {
                Genres = await _appDbContext.Genres.Where(g => !g.IsDeleted).ToListAsync(),
                Albums = await query.Skip((page - 1) * 4).
                Take(take).
                ToListAsync(),
                PageCount = pageCount,
                CurrentPage = page
            };

            return View(albumVM);
        }
        private int CalculatePageCount(List<Album> albums, int take)
        {
            return (int)Math.Ceiling((decimal)(albums.Count()) / take);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return NotFound();
            Album? checkAlbum = await _appDbContext.Albums.FirstOrDefaultAsync(a => a.Id == id);
            ViewBag.UserId = null;
            if (User.Identity.IsAuthenticated)
            {
                AppUser? user = await _userManager.FindByNameAsync(User.Identity.Name);
                ViewBag.UserId = user.Id;
            }
            if (checkAlbum == null)
            {
                return NotFound();
            }
#pragma warning disable CS8601 // Возможно, назначение-ссылка, допускающее значение NULL.
            AlbumVM albumVM = new()
            {
                Genres = await _appDbContext.Genres.Where(g => !g.IsDeleted).ToListAsync(),
                Album = await _appDbContext.Albums.Where(a => !a.IsDeleted).Include(a => a.Artist).Include(bd => bd.Comments).ThenInclude(u => u.User).Include(a => a.Genre).FirstOrDefaultAsync(a => a.Id == id),
                Songs = await _appDbContext.Songs.Where(a => a.AlbumId == id).ToListAsync()

            };
#pragma warning restore CS8601 // Возможно, назначение-ссылка, допускающее значение NULL.

            return View(albumVM);
        }
        [HttpPost]
        public async Task<IActionResult> AddComment(string Content, int albumId)
        {
            if (Content == null)
            {
                ModelState.AddModelError("", "Comment cannot be empty!");
                return RedirectToAction("Detail", new { id = albumId });
            }
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
        public async Task<IActionResult> DeleteComment(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Comment? comment = await _appDbContext.Comments.FirstOrDefaultAsync(b => b.Id == id);
            if (comment == null)
            {
                return NotFound();
            }
            _appDbContext.Comments.Remove(comment);
            _appDbContext.SaveChanges();
            return RedirectToAction("Detail", new { id = comment.AlbumId });
        }

        public async Task<IActionResult> AddSongPlaylist(int? SongId)
        {
            if (SongId==null)
            {
                return NotFound();
            }
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login","Account");
            }
            AppUser? user = await _userManager.FindByNameAsync(User.Identity.Name);
            UserSong? songCheck = await _appDbContext.UserSongs.FirstOrDefaultAsync(s=>s.Song.Id ==SongId);
            if (songCheck != null) {
                return RedirectToAction("Index");
            }
            UserSong userSong = new()
            {
                AppUserId = user.Id,
                SongId = (int)SongId,
            };
            await _appDbContext.UserSongs.AddAsync(userSong);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
