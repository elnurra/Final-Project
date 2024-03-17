using FinalProject.Areas.AdminArea.ViewModels.AccountCRUD;
using FinalProject.DAL;
using FinalProject.Extensions;
using FinalProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace FinalProject.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public AccountController(UserManager<AppUser> userManager, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            AppUser? user;
            if (User.Identity != null && User.Identity.IsAuthenticated && User.Identity.Name != null)
            {
                user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user == null)
                {
                    return NotFound();
                }
                return View(user);
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            if (id == null) return NotFound();
            AppUser? user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            AccountUpdateVM accountUpdateVM =new()
            {
                Email = user.Email,
                UserName = user.UserName,
                ImageUrl = user.ImageUrl,
                Fullname = user.Fullname
            };
             return View(accountUpdateVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(string id, AccountUpdateVM accountUpdateVM)
        {
            if (id == null) return NotFound();
            AppUser? user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            if (accountUpdateVM.Photo != null)
            {
                string fullpath = Path.Combine(_env.WebRootPath, "Admin/images/AdminImages", user.ImageUrl);
                if (System.IO.File.Exists(fullpath))
                {
                    System.IO.File.Delete(fullpath);
                }
                if (!accountUpdateVM.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Should be only photo extensions f.e: png,jpg");
                    return View(accountUpdateVM);
                }
                if (accountUpdateVM.Photo.CheckImageSize(500))
                {
                    ModelState.AddModelError("Photo", "Photo not be bigger than 500kb");
                    return View(accountUpdateVM);
                }
                user.ImageUrl = accountUpdateVM.Photo.SaveImage(_env, "Admin/images/AdminImages", accountUpdateVM.Photo.FileName);
                user.Fullname = accountUpdateVM.Fullname;
                user.UserName = accountUpdateVM.UserName;
                user.Email = accountUpdateVM.Email;
                await _userManager.UpdateAsync(user);
            }
            return RedirectToAction("Index");
        }
    }
}