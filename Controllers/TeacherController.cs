using Campus_Companion.Models;
using Campus_companion_Hackathon.Models;
using Campus_companion_Hackathon.Models;
using Microsoft.AspNetCore.Mvc;
namespace Campus_companion_Hackathon.Controllers
{
    public class TeacherController : Controller
    {
        private readonly AppDbContext _context;

        public TeacherController(AppDbContext context)
        {
            _context = context;
        }

        // GET
        public IActionResult Signup()
        {
            return View();
        }

        // POST
        [HttpPost]
        public IActionResult Signup(Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                _context.Teachers.Add(teacher);
                _context.SaveChanges();

                ViewBag.Message = "Signup Successful!";
                return View();
            }

            return View();
        }
    }
}

