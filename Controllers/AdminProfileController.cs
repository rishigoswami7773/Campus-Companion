using Microsoft.AspNetCore.Mvc;

namespace Campus_Companion.Controllers
{
    public class AdminProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
