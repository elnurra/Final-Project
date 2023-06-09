using FinalProject.DAL;
using FinalProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;

namespace FinalProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _appDbContext;
       
        public HomeController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;

        }

        public async  Task<IActionResult> Index()
        {
            
            List<Slider> sliders = await _appDbContext.Sliders.Where(s=>!s.IsDeleted).ToListAsync();

            return View(sliders);
        }

        public async Task<IActionResult> Subscribe(string Email)
        {
            if (Email==null)
            {
                return NotFound();
            }
            Subscribe? existEmail = await _appDbContext.Subscribes.FirstOrDefaultAsync(e=>e.Email==Email);
            if (existEmail != null)
            {
                ModelState.AddModelError("email","This email is already subscribed");
            }
            else
            {
                Subscribe subscribe = new();
                subscribe.Email = Email;
                await _appDbContext.Subscribes.AddAsync(subscribe);
                await _appDbContext.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}