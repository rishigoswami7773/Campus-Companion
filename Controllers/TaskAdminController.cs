using Microsoft.AspNetCore.Mvc;

namespace Campus_Companion.Controllers
{
    public class TaskAdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
