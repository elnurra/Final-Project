using FinalProject.Areas.AdminArea.ViewModels.AlbumCRUD;
using FinalProject.Areas.AdminArea.ViewModels.SliderCRUD;
using FinalProject.DAL;
using FinalProject.Extensions;
using FinalProject.Models;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;


namespace FinalProject.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class AlbumController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _env;

        public AlbumController(AppDbContext appDbContext, IWebHostEnvironment env)
        {
            _appDbContext = appDbContext;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var query = _appDbContext.Albums.Include(a => a.Artist).Include(g => g.Genre).Include(s => s.Songs);

            ViewBag.AlbumCount = query.Count();
            AlbumReadVM albumVM = new()
            {
                Genres = await _appDbContext.Genres.Where(g => !g.IsDeleted).ToListAsync(),
                Albums = await query.ToListAsync(),
            };
            return View(albumVM);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Genres = new SelectList(await _appDbContext.Genres.ToListAsync(), "Id", "Name");
            ViewBag.Artists = new SelectList(await _appDbContext.Artists.ToListAsync(), "Id", "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AlbumCreateVM albumCreateVM)
        {
            if (albumCreateVM.Photo == null)
            {
                ModelState.AddModelError("Photo", "Upploaded is empty");
                return View();
            }
            if (!albumCreateVM.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Should be only photo extensions f.e: png,jpg");
                return View();
            }
            if (albumCreateVM.Photo.CheckImageSize(500))
            {
                ModelState.AddModelError("Photo", "Photo not be bigger than 500kb");
                return View();
            }
            Album newAlbum = new()
            {
                Name = albumCreateVM.Name,
                ImageUrl = albumCreateVM.Photo.SaveImage(_env, "images", albumCreateVM.Photo.FileName),
                ArtistId = albumCreateVM.ArtistId,
                GenreId = albumCreateVM.GenreId,
                Label = albumCreateVM.Label,
                CreatedTime = albumCreateVM.CreatedTime,
                IsDeleted = false
            };
            await _appDbContext.Albums.AddAsync(newAlbum);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Update(int? id)
        {
            ViewBag.Genres = new SelectList(await _appDbContext.Genres.ToListAsync(), "Id", "Name");
            ViewBag.Artists = new SelectList(await _appDbContext.Artists.ToListAsync(), "Id", "Name");
            if (id == null) return NotFound();
            Album? album = await _appDbContext.Albums.FirstOrDefaultAsync(c => c.Id == id);
            if (album == null) return NotFound();
            return View(new AlbumUpdateVM 
            { ImageUrl = album.ImageUrl, 
                Name = album.Name, 
                CreatedTime = album.CreatedTime, 
                Label = album.Label, 
                GenreId = album.GenreId, 
                ArtistId = album.ArtistId
            });
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id,AlbumUpdateVM albumUpdateVM)
        {
            if (id == null) return NotFound();
            Album? album = await _appDbContext.Albums.FirstOrDefaultAsync(c => c.Id == id);
            if (album == null) return NotFound();
            if (albumUpdateVM.Photo != null)
            {
                string fullpath = Path.Combine(_env.WebRootPath, "images", album.ImageUrl);
                if (System.IO.File.Exists(fullpath))
                {
                    System.IO.File.Delete(fullpath);
                }
                if (!albumUpdateVM.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Should be only photo extensions f.e: png,jpg");
                    return View();
                }
                if (albumUpdateVM.Photo.CheckImageSize(500))
                {
                    ModelState.AddModelError("Photo", "Photo not be bigger than 500kb");
                    return View();
                }

                album.ImageUrl = albumUpdateVM.Photo.SaveImage(_env, "images", albumUpdateVM.Photo.FileName);
                album.Name = albumUpdateVM.Name;
                album.CreatedTime = albumUpdateVM.CreatedTime;
                album.ArtistId = albumUpdateVM.ArtistId;
                album.GenreId = albumUpdateVM.GenreId;
                album.Label = albumUpdateVM.Label;

                await _appDbContext.SaveChangesAsync();
            }
            return RedirectToAction("Index"); ;
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            Album? slider = await _appDbContext.Albums.FirstOrDefaultAsync(s => s.Id == id);
            if (slider == null)
            {
                NotFound();
            }
            else
            {
                _appDbContext.Remove(slider);
                _appDbContext.SaveChanges();
                string fullpath = Path.Combine(_env.WebRootPath, "images", slider.ImageUrl);
                if (System.IO.File.Exists(fullpath))
                {
                    System.IO.File.Delete(fullpath);
                }
            };
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return NotFound();
            Album? album = await _appDbContext.Albums.FirstOrDefaultAsync(s => s.Id == id);
            if (album == null) return NotFound();
            AlbumReadVM albumReadVM = new()
            {
                Genres = await _appDbContext.Genres.Where(g => !g.IsDeleted).ToListAsync(),
                Album = await _appDbContext.Albums.Where(a => !a.IsDeleted)
                .Include(a => a.Artist)
                .Include(bd => bd.Comments)
                .ThenInclude(u => u.User)
                .Include(a => a.Genre)
                .FirstOrDefaultAsync(a => a.Id == id),
                Songs = await _appDbContext.Songs.Where(a => a.AlbumId == id).ToListAsync()
            };
            return View(albumReadVM);
        }
    }
}
