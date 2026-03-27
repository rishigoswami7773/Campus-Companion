using Microsoft.AspNetCore.Mvc;

namespace Campus_Companion.Models
{
    public class Event : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
