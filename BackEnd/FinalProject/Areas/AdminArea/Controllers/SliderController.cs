using FinalProject.Areas.AdminArea.ViewModels.SliderCRUD;
using FinalProject.DAL;
using FinalProject.Extensions;
using FinalProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace FinalProject.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _env;

        public SliderController(AppDbContext appDbContext, IWebHostEnvironment env)
        {
            _appDbContext = appDbContext;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            List<Slider> sliders = await _appDbContext.Sliders.ToListAsync();
            return View(sliders);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SliderCreateVM sliderCreateVM)
        {
            if (sliderCreateVM.Photo == null)
            {
                ModelState.AddModelError("Photo", "Upploaded is empty");
                return View();
            }
            if (!sliderCreateVM.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Should be only photo extensions f.e: png,jpg");
                return View();
            }
            if (sliderCreateVM.Photo.CheckImageSize(500))
            {
                ModelState.AddModelError("Photo", "Photo not be bigger than 500kb");
                return View();
            }
            Slider newSlider = new()
            {
                ImageUrl = sliderCreateVM.Photo.SaveImage(_env, "images", sliderCreateVM.Photo.FileName),
                Title = sliderCreateVM.Title,
                Description = sliderCreateVM.Description
            };
            await _appDbContext.Sliders.AddAsync(newSlider);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        public async Task <IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();
            Slider? slider = await _appDbContext.Sliders.FirstOrDefaultAsync(c => c.Id == id);
            if (slider == null) return NotFound();
            return View(new SliderUpdateVM { ImageUrl = slider.ImageUrl, Description = slider.Description,Title=slider.Title });
        }

        [HttpPost]
        public IActionResult Update(int? id, SliderUpdateVM updateVM)
        {
            if (id == null) return NotFound();
            Slider? slider = _appDbContext.Sliders.FirstOrDefault(c => c.Id == id);
            if (slider == null) return NotFound();
            if (updateVM.Photo != null)
            {
                string fullpath = Path.Combine(_env.WebRootPath, "images", slider.ImageUrl);
                if (System.IO.File.Exists(fullpath))
                {
                    System.IO.File.Delete(fullpath);
                }
                if (!updateVM.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Should be only photo extensions f.e: png,jpg");
                    return View();
                }
                if (updateVM.Photo.CheckImageSize(500))
                {
                    ModelState.AddModelError("Photo", "Photo not be bigger than 500kb");
                    return View();
                }

                slider.ImageUrl = updateVM.Photo.SaveImage(_env, "images", updateVM.Photo.FileName);
                slider.Title = updateVM.Title;
                slider.Description = updateVM.Description;

                _appDbContext.SaveChanges();
            }
            return RedirectToAction("Index"); ;

        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return NotFound();
            Slider? slider = await _appDbContext.Sliders.FirstOrDefaultAsync(s => s.Id == id);
            if (slider == null) return NotFound();
            return View(slider);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            Slider? slider = await _appDbContext.Sliders.FirstOrDefaultAsync(s => s.Id == id);
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
    }
}
