using FinalProject.DAL;
using FinalProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    }
}