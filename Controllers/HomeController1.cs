using Microsoft.AspNetCore.Mvc;

namespace campus_companion.Controllers
{
    public class HomeController1 : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
