using FinalProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.ViewComponents
{
    public class NavbarViewComponent : ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;

        public NavbarViewComponent(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            AppUser? user;
            ViewBag.Fullname = string.Empty;
            if (User.Identity != null && User.Identity.IsAuthenticated && User.Identity.Name != null)
            {
                user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user != null)
                {
                    ViewBag.Fullname = user.Fullname;
                    return View(user);
                }

            }
                return View();
        }
    }
}