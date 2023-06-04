using FinalProject.DAL;
using FinalProject.Models;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    public class EventController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<AppUser> _userManager;

        public EventController(AppDbContext appDbContext, UserManager<AppUser> userManager)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
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
            EventVM eventVM = new();

            Event? Event = await _appDbContext.Events.Where(e => !e.IsDeleted).Include(e=>e.Artist).Include(bd => bd.Comments).ThenInclude(u => u.User).FirstOrDefaultAsync(e => e.Id == id);
            return View(Event);
        }

        public async Task<IActionResult> AddComment(string Content, int eventId)
        {
            AppUser? user;

            if (User.Identity.IsAuthenticated)
            {
                user = await _userManager.FindByNameAsync(User.Identity.Name);

            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            Comment comment = new()
            {
                CreatedTime = DateTime.Now,
                AppUserId = user.Id,
                EventId = eventId,
                Content = Content
            };
            _appDbContext.Comments.Add(comment);
            _appDbContext.SaveChanges();
            return RedirectToAction("Detail", new { id = eventId });
        }
        public async Task<IActionResult> DeleteComment(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Comment? comment = await _appDbContext.Comments.FirstOrDefaultAsync(b => b.Id == id);
            if (comment == null)
            {
                return NotFound();
            }
            _appDbContext.Comments.Remove(comment);
            _appDbContext.SaveChanges();
            return RedirectToAction("Detail", new { id = comment.EventId });
        }


    }
}
