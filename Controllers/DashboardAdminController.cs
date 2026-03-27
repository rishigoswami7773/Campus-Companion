using Microsoft.AspNetCore.Mvc;

using Campus_Companion.Filters;

namespace Campus_Companion.Controllers
{
    [AdminAuth]
    public class DashboardAdminController : Controller
    {
        public IActionResult Index()
        {
            // Pass the static tasks list from TaskAdminController
            return View(TaskAdminController.Tasks);
        }
    }
}
