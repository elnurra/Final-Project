using FinalProject.Areas.AdminArea.ViewModels.EventCRUD;
using FinalProject.Areas.AdminArea.ViewModels.SongCRUD;
using FinalProject.DAL;
using FinalProject.Extensions;
using FinalProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace FinalProject.Areas.AdminArea.Controllers
{

    [Area("AdminArea")]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class SongController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _env;

        public SongController(AppDbContext appDbContext, IWebHostEnvironment env)
        {
            _appDbContext = appDbContext;
            _env = env;
        }

        public async Task<IActionResult> Index(string search)
        {
            List<Song> songs = search != null ?
   await _appDbContext.Songs.Include(a => a.Album)
    .Where(u => u.Name.Trim().ToLower().Contains(search.Trim().ToLower()))
    .Where(u => !u.IsDeleted).ToListAsync()
    : await _appDbContext.Songs.Include(a => a.Album).ToListAsync();
            return View(songs);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Albums = new SelectList(await _appDbContext.Albums.ToListAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SongCreateVM songCreateVM)
        {
            if (songCreateVM.Audio == null)
            {
                ModelState.AddModelError("Song", "Upploaded is empty");
                return View();
            }
            if (!songCreateVM.Audio.IsAudio())
            {
                ModelState.AddModelError("Song", "Should be only Audio extensions f.e: mp3");
                return View();
            }
            if (songCreateVM.Audio.CheckAudioSize(5000000))
            {
                ModelState.AddModelError("Song", "Song should be not be bigger than 50mb");
                return View();
            }
            Song newSong = new()
            {
                Name = songCreateVM.Name,
                SongUrl = songCreateVM.Audio.SaveAudio(_env, "songs", songCreateVM.Audio.FileName),
                AlbumId = songCreateVM.AlbumId,
                IsDeleted = false
            };
            await _appDbContext.Songs.AddAsync(newSong);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            ViewBag.Albums = new SelectList(await _appDbContext.Albums.ToListAsync(), "Id", "Name");
            if (id == null) return NotFound();
            Song? song = await _appDbContext.Songs.FirstOrDefaultAsync(c => c.Id == id);
            if (song == null) return NotFound();
            return View(new SongUpdateVM
            {
                SongUrl = song.SongUrl,
                Name = song.Name,
                AlbumId = song.AlbumId
            });
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, SongUpdateVM songUpdateVM)
        {
            if (id == null) return NotFound();
            Song? song = await _appDbContext.Songs.FirstOrDefaultAsync(s => s.Id == id);
            if (song == null) return NotFound();
            if (songUpdateVM.Audio == null)
            {
                ModelState.AddModelError("Photo", "Upploaded is empty");
                return View();
            }
            if (!songUpdateVM.Audio.IsAudio())
            {
                ModelState.AddModelError("Photo", "Should be only photo extensions f.e: png,jpg");
                return View();
            }
            if (songUpdateVM.Audio.CheckAudioSize(5000000))
            {
                ModelState.AddModelError("Photo", "Photo not be bigger than 500kb");
                return View();
            }
            song.Name = songUpdateVM.Name;
            song.SongUrl = songUpdateVM.Audio.SaveAudio(_env, "songs", songUpdateVM.Audio.FileName);
            song.AlbumId = songUpdateVM.AlbumId;
            return View();
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            Song? song = await _appDbContext.Songs.FirstOrDefaultAsync(s => s.Id == id);
            if (song == null)
            {
                NotFound();
            }
            else
            {
                _appDbContext.Remove(song);
                await _appDbContext.SaveChangesAsync();
                string fullpath = Path.Combine(_env.WebRootPath, "songs", song.SongUrl);
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
            Song? song = await _appDbContext.Songs.Include(a => a.Album).FirstOrDefaultAsync(s => s.Id == id);
            if (song == null) return NotFound();
            return View(song);
        }
    }

}
