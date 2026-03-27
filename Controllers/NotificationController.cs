using Microsoft.AspNetCore.Mvc;

namespace Campus_Companion.Controllers
{
    public class NotificationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
