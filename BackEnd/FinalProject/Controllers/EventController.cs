using FinalProject.DAL;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    public class EventController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public EventController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<IActionResult> Index()
        {
            List<Event> events = await _appDbContext.Events.Where(e => !e.IsDeleted).ToListAsync();
            return View(events);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return NotFound();
            Event? checkEvent = await _appDbContext.Events.Where(e => !e.IsDeleted).FirstOrDefaultAsync(e=>e.Id==id);
            if (checkEvent == null) return NotFound();
            Event? Event = await _appDbContext.Events.Where(e => !e.IsDeleted).Include(e=>e.Artist).FirstOrDefaultAsync(e => e.Id == id);
            return View(Event);
        }
    }
}
