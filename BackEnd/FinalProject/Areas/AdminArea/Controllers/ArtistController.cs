using FinalProject.Areas.AdminArea.ViewModels.ArtistCRUD;
using FinalProject.DAL;
using FinalProject.Models;
using Microsoft.AspNetCore.Authorization;
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

        public async Task<IActionResult> Index()
        {
            List<Artist> artists = await _appDbContext.Artists.ToListAsync();
            return View(artists);
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
                Name = artistCreateVM.Name
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
            return View(new ArtistUpdateVM { Name = artist.Name });
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
            _appDbContext.SaveChanges();
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
    }
}
