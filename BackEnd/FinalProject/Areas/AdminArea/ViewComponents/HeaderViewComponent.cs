using FinalProject.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Areas.AdminArea.ViewComponents
{
    public class HeaderViewComponent:ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;

        public HeaderViewComponent(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.Fullname = string.Empty;
#pragma warning disable CS8602 // Разыменование вероятной пустой ссылки.
            if (User.Identity.IsAuthenticated)
            {
#pragma warning disable CS8604 // Возможно, аргумент-ссылка, допускающий значение NULL.
                AppUser? user = await _userManager.FindByNameAsync(User.Identity.Name);
#pragma warning restore CS8604 // Возможно, аргумент-ссылка, допускающий значение NULL.
                ViewBag.Fullname = user.Fullname;
                return View(user);
            }
#pragma warning restore CS8602 // Разыменование вероятной пустой ссылки.
            return View();
        }
    }
}
