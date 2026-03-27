using Microsoft.AspNetCore.Mvc;

namespace Campus_Companion.Controllers
{
    public class EventAdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
