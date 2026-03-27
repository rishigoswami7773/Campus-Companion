using Microsoft.AspNetCore.Mvc;

namespace Campus_Companion.Controllers
{
    //[Area("Admin")]

    public class DashboardAdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
