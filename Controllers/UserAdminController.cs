using Microsoft.AspNetCore.Mvc;

namespace Campus_Companion.Controllers
{
    public class UserAdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
