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
            ViewBag.Fullname = string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                AppUser? user = await _userManager.FindByNameAsync(User.Identity.Name);
                ViewBag.Fullname = user.Fullname;
            }
            return View();
        }
    }
}