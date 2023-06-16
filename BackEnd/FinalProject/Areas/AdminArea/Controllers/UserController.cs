using FinalProject.Areas.AdminArea.ViewModels.UserCRUD;
using FinalProject.Models;
using FinalProject.ViewModels.AccountVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;
using System.Data;

namespace FinalProject.Areas.AdminArea.Controllers
{

    [Area("AdminArea")]
    [Authorize(Roles = "SuperAdmin")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        public UserController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        public async Task<IActionResult> Index(string search, int page = 1, int take = 4)
        {
            List<AppUser> userCount = await _userManager.Users.ToListAsync();
            var users = search != null ?
               _userManager.Users
                .Where(u => u.Fullname.Trim().ToLower().Contains(search.Trim().ToLower()))
                : _userManager.Users.Skip((page - 1) * 4).Take(take);
           int pageCount = CalculatePageCount(userCount, take);
            UserReadVM userReadVM = new()
            {
                AppUsers = await users.ToListAsync(),
                PageCount = pageCount,
                CurrentPage = page
            };
            return View(userReadVM);
        }

        [HttpGet]
        public async  Task<IActionResult> Create()
        {
            List<IdentityRole> roles = await _roleManager.Roles.ToListAsync();
            UserCreateVM userCreateVM = new()
            {
                AllRoles = roles
            };
            return View(userCreateVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserCreateVM userCreateVM, List<string> roles)
        {
            
            if (!ModelState.IsValid) return View(userCreateVM);
            List<IdentityRole> allRoles = await _roleManager.Roles.ToListAsync();
            AppUser user = new()
            {
                Fullname = userCreateVM.Fullname,
                UserName = userCreateVM.Username,
                Email = userCreateVM.Email,
                EmailConfirmed = true,
            };
            IdentityResult result = await _userManager.CreateAsync(user, userCreateVM.Password);
            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                userCreateVM.AllRoles = allRoles;
                return View(userCreateVM);
            }
            await _userManager.GenerateEmailConfirmationTokenAsync(user);
            await _userManager.AddToRolesAsync(user, roles);
            await _signInManager.SignInAsync(user, true);
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Detail(string id)
        {
            AppUser? user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            return View(new UserDetailVM
            {
                User = user,
                UserRoles = await _userManager.GetRolesAsync(user)
            });
        }
        public async Task<IActionResult> UpdateRole(string id)
        {
            AppUser? user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            return View(new UserUpdateRoleVM
            {
                User = user,
                UserRoles = await _userManager.GetRolesAsync(user),
                AllRoles = await _roleManager.Roles.ToListAsync()
            });
        }
        [HttpPost]
        public async Task<IActionResult> UpdateRole(string id, List<string> roles)
        {
            AppUser? user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            var userRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, userRoles);
            await _userManager.AddToRolesAsync(user, roles);
            return RedirectToAction("Index", "User");
        }
        public async Task<IActionResult> Delete(string id)
        {
            if (id==null) return NotFound();
            AppUser? user =  await _userManager.FindByIdAsync(id);
            if(user == null) return NotFound();
            await _userManager.DeleteAsync(user);
            return RedirectToAction("Index");

        }
        private int CalculatePageCount(List<AppUser> appUsers, int take)
        {
            return (int)Math.Ceiling((decimal)(appUsers.Count()) / take);
        }
    }
}
