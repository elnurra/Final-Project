using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Areas.AdminArea.Controllers
{
    public class EventController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
