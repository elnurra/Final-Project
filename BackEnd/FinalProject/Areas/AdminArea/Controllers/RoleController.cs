using FinalProject.Areas.AdminArea.ViewModels.RoleCRUD;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace FinalProject.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    [Authorize(Roles = "SuperAdmin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task <IActionResult> Index(string search)
        {
#pragma warning disable CS8602 // Разыменование вероятной пустой ссылки.
            List<IdentityRole> roles = search != null ?
  await _roleManager.Roles
   .Where(u => u.Name.Trim()
   .ToLower()
   .Contains(search.Trim().ToLower())).ToListAsync()
   : await _roleManager.Roles.ToListAsync();
#pragma warning restore CS8602 // Разыменование вероятной пустой ссылки.
            return View(roles);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(string roleName)
        {
            if (!string.IsNullOrEmpty(roleName))
            {
                var reseult = await _roleManager.CreateAsync(new IdentityRole { Name = roleName });
                if (reseult.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }
            return View();
        }
        public async Task<IActionResult> Delete(string id)
        {
            if (!ModelState.IsValid) return NotFound();
            IdentityRole? result = await _roleManager.FindByIdAsync(id);
            if (result != null)
            {
                await _roleManager.DeleteAsync(result);
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task <IActionResult> Update(string id)
        {
            if (!ModelState.IsValid) return NotFound();
            IdentityRole? result = await _roleManager.FindByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return View(new RoleUpdateVM() { Name = result.Name});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(string id, RoleUpdateVM roleUpdateVM)
        {
            if (!ModelState.IsValid) return NotFound();
            IdentityRole? checkRole = await _roleManager.FindByIdAsync(id);
            if (checkRole ==null) return NotFound();
            checkRole.Name = roleUpdateVM.Name;
            await _roleManager.UpdateAsync(checkRole);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Detail (string id)
        {
            if(!ModelState.IsValid) return NotFound();
            IdentityRole? result = await _roleManager.FindByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }




    }
}
