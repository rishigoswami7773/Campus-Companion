using Microsoft.AspNetCore.Mvc;

namespace Campus_Companion.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(IFormCollection form)
        {
            // Simple validation simulation
            string password = form["password"];
            string confirmPassword = form["confirmPassword"];

            if (password != confirmPassword)
            {
                ViewBag.Error = "Passwords do not match.";
                return View();
            }

            // Simulate successful registration and redirect to login
            TempData["SuccessMessage"] = "Registration successful! Please login.";
            return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public IActionResult Index(string email, string password)
        {
            if (email == "admin@gmail.com" && password == "admin123")
            {
                // Set lightweight cookie auth token mock
                CookieOptions options = new CookieOptions
                {
                    Expires = System.DateTimeOffset.Now.AddDays(1)
                };
                Response.Cookies.Append("IsAdmin", "true", options);
                
                return RedirectToAction("Index", "DashboardAdmin");
            }
            
            ViewBag.Error = "Invalid email or password.";
            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("IsAdmin");
            return RedirectToAction("Index", "Home");
        }
    }
}
