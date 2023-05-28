using FinalProject.DAL;
using FinalProject.Models;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.ViewComponents
{
    public class EventViewComponent : ViewComponent
    {
        private readonly AppDbContext _appDbContext;

        public EventViewComponent(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            EventVM eventVM = new()
            {
                Events = await _appDbContext.Events.Where(e => !e.IsDeleted).ToListAsync(),
                Genres = await _appDbContext.Genres.Where(g=>!g.IsDeleted).Take(6).ToListAsync()
            };


            return View(eventVM);

        }
    }
}
