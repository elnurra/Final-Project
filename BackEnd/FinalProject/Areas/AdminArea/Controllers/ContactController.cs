using FinalProject.Areas.AdminArea.ViewModels.ContactCRUD;
using FinalProject.DAL;
using FinalProject.Models;
using FinalProject.Services;
using FinalProject.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace FinalProject.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class ContactController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IFileService _fileService;
        public ContactController(AppDbContext appDbContext, UserManager<AppUser> userManager, IEmailService emailService, IFileService fileService)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
            _emailService = emailService;
            _fileService = fileService;
        }

        public async Task<IActionResult> Index(string search, int page = 1, int take = 4)
        {
            List<Contact> contactCount = await _appDbContext.Contacts.ToListAsync();
            var contacts = search != null ?
            _appDbContext.Contacts
                .Where(u => u.Name.Trim().ToLower().Contains(search.Trim().ToLower()))
                : _appDbContext.Contacts.Skip((page - 1) * 4).Take(take);
            int pageCount = CalculatePageCount(contactCount, take);
            ContactReadVM genreReadVM = new()
            {
                CurrentPage = page,
                PageCount = pageCount,
               Contacts = await contacts.ToListAsync()
            };

            return View(genreReadVM);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            Contact? contact = await _appDbContext.Contacts.FirstOrDefaultAsync(c => c.Id == id);
            if (contact == null) return NotFound();
             _appDbContext.Contacts.Remove(contact);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return NotFound();
            Contact? contact = await _appDbContext.Contacts.FirstOrDefaultAsync(c => c.Id == id);
            if (contact == null) return NotFound();
            return View(contact);
        }
        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();
            Contact? contact = await _appDbContext.Contacts.FirstOrDefaultAsync(c => c.Id == id);
            if (contact == null) return NotFound();

            return View(contact);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, string response)
        {
            if (id == null) return NotFound();
            Contact? contact = await _appDbContext.Contacts.FirstOrDefaultAsync(c => c.Id == id);
            if (contact == null) return NotFound();
            string body = string.Empty;

            string path = "wwwroot/Template/Contact.html";
            body = _fileService.ReadFile(path, body);
            body = body.Replace("{{Response}}", response);

            string subject = "Contact Email";
            _emailService.Send(contact.Email, subject, body);
            return View(contact);
        }

        private int CalculatePageCount(List<Contact> genres, int take)
        {
            return (int)Math.Ceiling((decimal)(genres.Count()) / take);
        }

        
    }
}
