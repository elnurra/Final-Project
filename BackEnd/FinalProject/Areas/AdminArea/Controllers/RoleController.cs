using FinalProject.Areas.AdminArea.ViewModels.RoleCRUD;
using FinalProject.Models;
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

        public async Task <IActionResult> Index(string search, int page = 1, int take = 4)
        {
            List<IdentityRole> roleCount = await _roleManager.Roles.ToListAsync();
#pragma warning disable CS8602 // Разыменование вероятной пустой ссылки.
            var roles = search != null ?
  _roleManager.Roles
   .Where(u => u.Name.Trim()
   .ToLower()
   .Contains(search.Trim().ToLower()))
   :  _roleManager.Roles.Skip((page - 1) * 4).Take(take);
#pragma warning restore CS8602 // Разыменование вероятной пустой ссылки.
            int pageCount = CalculatePageCount(roleCount, take);
            RoleReadVM roleReadVM = new()
            {
                IdentityRoles = await roles.ToListAsync(),
                PageCount = pageCount,
                CurrentPage = page
            };
            return View(roleReadVM);
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
            ModelState.AddModelError("Role", "Role should not be empty");
            return View(roleName) ;
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
            RoleUpdateVM roleUpdateVM = new();
            if (!ModelState.IsValid) return NotFound();
            IdentityRole? result = await _roleManager.FindByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            if (result.Name !=null)
            {
                
                roleUpdateVM.Name = result.Name;
            }

            return View(roleUpdateVM);
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
        private static int CalculatePageCount(List<IdentityRole> roles, int take)
        {
            return (int)Math.Ceiling((decimal)(roles.Count) / take);
        }



    }
}
