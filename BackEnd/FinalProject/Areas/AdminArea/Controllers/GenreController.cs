using FinalProject.Areas.AdminArea.ViewModels.GenreCRUD;
using FinalProject.DAL;
using FinalProject.Models;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FinalProject.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class GenreController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public GenreController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IActionResult> Index(string search, int page = 1, int take = 4)
        {
            List<Genre> genreCount = await _appDbContext.Genres.ToListAsync();
            var genres = search != null ?
                _appDbContext.Genres
                .Where(u => u.Name.Trim().ToLower().Contains(search.Trim().ToLower()))
                : _appDbContext.Genres.Skip((page - 1) * 4).Take(take);
            int pageCount = CalculatePageCount(genreCount,take);
            GenreReadVM genreReadVM = new()
            {
                CurrentPage = page,
                PageCount = pageCount,
                Genres = await genres.ToListAsync()
            };

            return View(genreReadVM);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(GenreCreateVM genre)
        {
            if (!ModelState.IsValid) return View();
            bool isExistGenre = _appDbContext.Genres.Any(c => c.Name == genre.Name);
            if (isExistGenre)
            {
                ModelState.AddModelError("Name", "This name is already exist");
                return View();
            }
            Genre newGenre = new()
            {
                Name = genre.Name,
                IsDeleted = genre.IsDeleted,
            };
            _appDbContext.Genres.Add(newGenre);
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();
            Genre? genre = await _appDbContext.Genres.FirstOrDefaultAsync(c => c.Id == id);
            if (genre == null) return NotFound();
            return View(new GenreUpdateVM { Name = genre.Name });
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, GenreUpdateVM updateVM)
        {

            if (id == null) return NotFound();
            Genre? existGenre = await _appDbContext.Genres.FirstOrDefaultAsync(c => c.Id == id);
            if (!ModelState.IsValid) return View();
            bool isExistGenre = await _appDbContext.Genres.AnyAsync(c => c.Name.ToLower() == updateVM.Name.ToLower() && c.Id != id);
            if (isExistGenre)
            {
                ModelState.AddModelError("Name", "This name is already exist");
                return View();
            }
            if (existGenre == null) return NotFound();
            existGenre.Name = updateVM.Name;
            existGenre.IsDeleted = updateVM.IsDeleted;
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task <IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            Genre? genre = await _appDbContext.Genres.FirstOrDefaultAsync(c => c.Id == id);
            if (genre == null) return NotFound();
            _appDbContext.Genres.Remove(genre);
           await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return NotFound();            
            Genre? genre = await _appDbContext.Genres.FirstOrDefaultAsync(c => c.Id == id);
            if (genre == null) return NotFound();
            return View(genre);
        }
        private int CalculatePageCount(List<Genre> genres, int take)
        {
            return (int)Math.Ceiling((decimal)(genres.Count()) / take);
        }
    }

}
