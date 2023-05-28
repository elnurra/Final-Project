using FinalProject.DAL;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    public class CommonController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public CommonController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task <IActionResult> Search(string search)
        {
            List<Album> albums = await _appDbContext.Albums.
                Where(a => a.Name.ToLower().Contains(search.ToLower()))
                .Take(3)
                .OrderBy(a => a.Id)
                .ToListAsync();
            return PartialView("_SearchPartial", albums);
        }
    }
}
