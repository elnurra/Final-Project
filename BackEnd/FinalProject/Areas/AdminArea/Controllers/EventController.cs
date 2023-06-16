using FinalProject.Areas.AdminArea.ViewModels.AlbumCRUD;
using FinalProject.Areas.AdminArea.ViewModels.EventCRUD;
using FinalProject.DAL;
using FinalProject.Extensions;
using FinalProject.Models;
using FinalProject.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using System.Data;

namespace FinalProject.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class EventController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _env;
        private readonly IEmailService _emailService;
        private readonly IFileService _fileService;

        public EventController(AppDbContext appDbContext, IWebHostEnvironment env, IEmailService emailService, IFileService fileService)
        {
            _appDbContext = appDbContext;
            _env = env;
            _emailService = emailService;
            _fileService = fileService;
        }


        public async Task<IActionResult> Index(string search,int page = 1, int take = 4)
        {
            List<Event> eventCount = await _appDbContext.Events.ToListAsync();
            var events = search != null ?
               _appDbContext.Events.Include(a => a.Artist)
                .Where(u => u.Title.Trim().ToLower().Contains(search.Trim().ToLower()))
                : _appDbContext.Events.Skip((page - 1) * 4).Take(take).Include(a => a.Artist);
            int pageCount = CalculatePageCount(eventCount, take);
            EventReadVM eventReadVM = new()
            {
                Events = await events.ToListAsync(),
                PageCount = pageCount,
                CurrentPage = page,
            };

            return View(eventReadVM);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Artists = new SelectList(await _appDbContext.Artists.ToListAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EventCreateVM eventCreateVM)
        {
            ViewBag.Artists = new SelectList(await _appDbContext.Artists.ToListAsync(), "Id", "Name");
            if (eventCreateVM.Photo == null)
            {
                ModelState.AddModelError("Photo", "Upploaded is empty");
                return View();
            }
            if (!eventCreateVM.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Should be only photo extensions f.e: png,jpg");
                return View();
            }
            if (eventCreateVM.Photo.CheckImageSize(500))
            {
                ModelState.AddModelError("Photo", "Photo not be bigger than 500kb");
                return View();
            }
            Event newEvent = new()
            {
                Title = eventCreateVM.Title,
                Address = eventCreateVM.Address,
                Description = eventCreateVM.Description,
                ImageUrl = eventCreateVM.Photo.SaveImage(_env, "images", eventCreateVM.Photo.FileName),
                ArtistId = eventCreateVM.ArtistId,
                AddressUrl = eventCreateVM.AddressUrl,
                Price = eventCreateVM.Price,
                CreatedTime = eventCreateVM.CreatedTime,
                IsDeleted = eventCreateVM.IsDeleted
            };

            await _appDbContext.Events.AddAsync(newEvent);
            await _appDbContext.SaveChangesAsync();
            string link = "https://localhost:44365/Event";
            List<Subscribe> subscribes = await _appDbContext.Subscribes.Where(s=>!s.IsDeleted).ToListAsync();
            string body = string.Empty;
            string path = "wwwroot/Template/Letter.html";
            body = _fileService.ReadFile(path, body);
            body = body.Replace("{{link}}", link);
            string subject = "New EVENT!";
            foreach (Subscribe item in subscribes)
            {
                _emailService.Send(item.Email, subject, body);
            }
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Update(int? id)
        {
            ViewBag.Artists = new SelectList(await _appDbContext.Artists.ToListAsync(), "Id", "Name");
            if (id == null) return NotFound();
            Event? @event = await _appDbContext.Events.FirstOrDefaultAsync(c => c.Id == id);
            if (@event == null) return NotFound();
            return View(new EventUpdateVM
            {
                ImageUrl = @event.ImageUrl,
                Title = @event.Title,
                CreatedTime = @event.CreatedTime,
                Description = @event.Description,
                Address = @event.Address,
                AddressUrl = @event.AddressUrl,
                Price = @event.Price,
                ArtistId = @event.ArtistId,
                IsDeleted = @event.IsDeleted,

            });
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, EventUpdateVM eventUpdateVM)
        {
            ViewBag.Artists = new SelectList(await _appDbContext.Artists.ToListAsync(), "Id", "Name");
            if (id == null) return NotFound();
            Event? @event = await _appDbContext.Events.FirstOrDefaultAsync(c => c.Id == id);
            if (@event == null) return NotFound();
            if (eventUpdateVM.Photo != null)
            {
                string fullpath = Path.Combine(_env.WebRootPath, "images", @event.ImageUrl);
                if (System.IO.File.Exists(fullpath))
                {
                    System.IO.File.Delete(fullpath);
                }
                if (!eventUpdateVM.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Should be only photo extensions f.e: png,jpg");
                    return View();
                }
                if (eventUpdateVM.Photo.CheckImageSize(500))
                {
                    ModelState.AddModelError("Photo", "Photo not be bigger than 500kb");
                    return View();
                }
                @event.ArtistId = eventUpdateVM.ArtistId;
                @event.ImageUrl = eventUpdateVM.Photo.SaveImage(_env, "images", eventUpdateVM.Photo.FileName);
                @event.Title = eventUpdateVM.Title;
                @event.Description = eventUpdateVM.Description;
                @event.CreatedTime = eventUpdateVM.CreatedTime;
                @event.Address = eventUpdateVM.Address;
                @event.AddressUrl = eventUpdateVM.AddressUrl;
                @event.Price = eventUpdateVM.Price;
                @event.IsDeleted = eventUpdateVM.IsDeleted;
                await _appDbContext.SaveChangesAsync();
            }
            return RedirectToAction("Index"); ;
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            Event? @event = await _appDbContext.Events.FirstOrDefaultAsync(s => s.Id == id);
            if (@event == null)
            {
                NotFound();
            }
            else
            {
                _appDbContext.Remove(@event);
                await _appDbContext.SaveChangesAsync();
                string fullpath = Path.Combine(_env.WebRootPath, "images", @event.ImageUrl);
                if (System.IO.File.Exists(fullpath))
                {
                    System.IO.File.Delete(fullpath);
                }
            };
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return NotFound();
            Event? @event = await _appDbContext.Events.Include(a=>a.Artist).FirstOrDefaultAsync(s => s.Id == id);
            if (@event == null) return NotFound();           
            return View(@event);
        }
        private int CalculatePageCount(List<Event> events, int take)
        {
            return (int)Math.Ceiling((decimal)(events.Count()) / take);
        }

    }
}
