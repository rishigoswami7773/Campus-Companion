using Microsoft.AspNetCore.Mvc;

namespace Campus_Companion.Controllers
{
    public class EventController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
