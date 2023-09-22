using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Dashboard.Controllers
{
    [Area(nameof(Dashboard))]
    [Authorize(Roles ="Admin,Moderator")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
