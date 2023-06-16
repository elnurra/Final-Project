using FinalProject.Areas.AdminArea.ViewModels.AlbumCRUD;
using FinalProject.Areas.AdminArea.ViewModels.ArtistCRUD;
using FinalProject.DAL;
using FinalProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace FinalProject.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class ArtistController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public ArtistController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IActionResult> Index(string search, int page = 1, int take = 4)
        {
            List<Artist> artistCount = await _appDbContext.Artists.ToListAsync();
            var artists = search != null ?
                _appDbContext.Artists
                .Where(u => u.Name.Trim().ToLower().Contains(search.Trim().ToLower()))
                : _appDbContext.Artists.Skip((page - 1) * 4).Take(take);
            int pageCount = CalculatePageCount(artistCount, take);
            ArtistReadVM artistReadVM = new()
            {
                Artists = await artists.ToListAsync(),
                PageCount = pageCount,
                CurrentPage = page
            };
            return View(artistReadVM);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(ArtistCreateVM artistCreateVM)
        {
            if (!ModelState.IsValid) return View();
            bool isExistArtist = _appDbContext.Artists.Any(c => c.Name == artistCreateVM.Name);
            if (isExistArtist)
            {
                ModelState.AddModelError("Name", "This name is already exist");
                return View();
            }
            Artist newArtist = new()
            {
                Name = artistCreateVM.Name,
                IsDeleted = artistCreateVM.IsDeleted
            };
            _appDbContext.Artists.Add(newArtist);
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();
            Artist? artist = await _appDbContext.Artists.SingleOrDefaultAsync(c => c.Id == id);
            if (artist == null) return NotFound();
            return View(new ArtistUpdateVM
            {
                Name = artist.Name,
                IsDeleted = artist.IsDeleted
            });
        }
        public async Task<IActionResult> Update(int? id, ArtistUpdateVM updateVM)
        {
            if (id == null) return NotFound();
            Artist? existArtist = await _appDbContext.Artists.FirstOrDefaultAsync(c => c.Id == id);
            if (!ModelState.IsValid) return View();
            bool isExistGenre = await _appDbContext.Artists.AnyAsync(c => c.Name.ToLower() == updateVM.Name.ToLower() && c.Id != id);
            if (isExistGenre)
            {
                ModelState.AddModelError("Name", "This name is already exist");
                return View();
            }
            if (existArtist == null) return NotFound();
            existArtist.Name = updateVM.Name;
            existArtist.IsDeleted = updateVM.IsDeleted;
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index");

        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            Artist? artist = await _appDbContext.Artists.FirstOrDefaultAsync(c => c.Id == id);
            if (artist == null)
            {
                return NotFound();
            }
            else
            {
                _appDbContext.Artists.Remove(artist);
                await _appDbContext.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return NotFound();
            Artist? artist = await _appDbContext.Artists.FirstOrDefaultAsync(c => c.Id == id);
            if (artist == null) return NotFound();
            return View(artist);
        }
        private int CalculatePageCount(List<Artist> artists, int take)
        {
            return (int)Math.Ceiling((decimal)(artists.Count()) / take);
        }
    }
}
