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

        public async Task<IActionResult> Index(string search, int page = 1, int take = 4)
        {
            List<Song> songCount = await _appDbContext.Songs.ToListAsync();
            var songs = search != null ?
    _appDbContext.Songs.Include(a => a.Album)
    .Where(u => u.Name.Trim().ToLower().Contains(search.Trim().ToLower()))
    : _appDbContext.Songs.Skip((page - 1) * 4).Take(take).Include(a => a.Album);

            int pageCount = CalculatePageCount(songCount, take);
            SongReadVM songReadVM = new()
            {
                PageCount = pageCount,
                CurrentPage = page,
                Songs = await songs.ToListAsync()
            };
            return View(songReadVM);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Albums = new SelectList(await _appDbContext.Albums.ToListAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SongCreateVM songCreateVM)
        {
            ViewBag.Albums = new SelectList(await _appDbContext.Albums.ToListAsync(), "Id", "Name");
            if (songCreateVM.Audio == null)
            {
                ModelState.AddModelError("Audio", "Upploaded is empty");
                return View(songCreateVM);
            }
            if (!songCreateVM.Audio.IsAudio())
            {
                ModelState.AddModelError("Audio", "Should be only Audio extensions f.e: mp3");
                return View(songCreateVM);
            }
            if (songCreateVM.Audio.CheckAudioSize(20000))
            {
                ModelState.AddModelError("Audio", "Song should be not be bigger than 20mb");
                return View(songCreateVM);
            }
            Song newSong = new()
            {
                Name = songCreateVM.Name,
                SongUrl = songCreateVM.Audio.SaveAudio(_env, "songs", songCreateVM.Audio.FileName),
                AlbumId = songCreateVM.AlbumId,
                IsDeleted = songCreateVM.IsDeleted
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
                AlbumId = song.AlbumId,
                IsDeleted= song.IsDeleted
            });
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, SongUpdateVM songUpdateVM)
        {
            ViewBag.Albums = new SelectList(await _appDbContext.Albums.ToListAsync(), "Id", "Name");
            if (id == null) return NotFound();
            Song? song = await _appDbContext.Songs.FirstOrDefaultAsync(s => s.Id == id);
            if (song == null) return NotFound();
            if (songUpdateVM.Audio == null)
            {
                ModelState.AddModelError("Audio", "Upploaded is empty");
                return View(songUpdateVM);
            }
            if (!songUpdateVM.Audio.IsAudio())
            {
                ModelState.AddModelError("Audio", "Should be only photo extensions f.e: mp3");
                return View(songUpdateVM);
            }
            if (songUpdateVM.Audio.CheckAudioSize(20000))
            {
                ModelState.AddModelError("Audio", "Song not be bigger than 20mb");
                return View(songUpdateVM);
            }
            song.Name = songUpdateVM.Name;
            song.SongUrl = songUpdateVM.Audio.SaveAudio(_env, "songs", songUpdateVM.Audio.FileName);
            song.AlbumId = songUpdateVM.AlbumId;
            song.IsDeleted = songUpdateVM.IsDeleted;
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
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
        private int CalculatePageCount(List<Song> songs, int take)
        {
            return (int)Math.Ceiling((decimal)(songs.Count()) / take);
        }
    }

}
