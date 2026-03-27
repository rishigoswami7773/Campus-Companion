using Campus_Companion.Data;
using Campus_Companion.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Campus_Companion.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public AccountController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var student = _context.Students.FirstOrDefault(s => s.Email == model.Email && s.Password == model.Password);
            if (student is null)
            {
                ViewBag.Error = "Invalid email or password.";
                return View(model);
            }

            HttpContext.Session.SetString("StudentName", student.Name);
            HttpContext.Session.SetInt32("StudentId", student.StudentId);

            return RedirectToAction("Index", "StudentDashboard");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (_context.Students.Any(s => s.Email == model.Email))
            {
                ModelState.AddModelError(nameof(model.Email), "Email already registered.");
                return View(model);
            }

            var student = new Student
            {
                Name = model.Name,
                Email = model.Email,
                Password = model.Password
            };
            _context.Students.Add(student);
            _context.SaveChanges();

            TempData["AuthSuccess"] = "Registration successful. Please login.";
            return RedirectToAction(nameof(Login));
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ForgotPassword(string Email)
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                ViewBag.Error = "Please enter a valid email address.";
                return View();
            }

            ViewBag.Message = "If an account exists for this email, a password reset link has been sent.";
            return View();
        }

        private int? GetStudentIdOrNull()
        {
            return HttpContext.Session.GetInt32("StudentId");
        }

        [HttpGet]
        public IActionResult Profile()
        {
            var studentId = GetStudentIdOrNull();
            if (!studentId.HasValue)
            {
                return RedirectToAction(nameof(Login));
            }

            var student = _context.Students.AsNoTracking().FirstOrDefault(s => s.StudentId == studentId.Value);
            if (student is null)
            {
                HttpContext.Session.Clear();
                return RedirectToAction(nameof(Login));
            }

            var profileImageUrl = GetProfileImageUrl(student.StudentId);

            ViewBag.ProfileImageUrl = profileImageUrl;

            return View(new ProfileViewModel
            {
                Name = student.Name,
                Email = student.Email
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Profile(ProfileViewModel model, IFormFile? profileImage)
        {
            var studentId = GetStudentIdOrNull();
            if (!studentId.HasValue)
            {
                return RedirectToAction(nameof(Login));
            }

            var student = _context.Students.FirstOrDefault(s => s.StudentId == studentId.Value);
            if (student is null)
            {
                HttpContext.Session.Clear();
                return RedirectToAction(nameof(Login));
            }

            if (!ModelState.IsValid)
            {
                ViewBag.ProfileImageUrl = GetProfileImageUrl(student.StudentId);
                return View(model);
            }

            // Email change safety: allow if same email, otherwise require uniqueness.
            if (!string.Equals(student.Email, model.Email, StringComparison.OrdinalIgnoreCase))
            {
                if (_context.Students.Any(s => s.Email == model.Email && s.StudentId != student.StudentId))
                {
                    ModelState.AddModelError(nameof(model.Email), "Email already in use.");
                    ViewBag.ProfileImageUrl = GetProfileImageUrl(student.StudentId);
                    return View(model);
                }
                student.Email = model.Email.Trim();
            }

            student.Name = model.Name.Trim();

            if (!string.IsNullOrWhiteSpace(model.NewPassword))
            {
                student.Password = model.NewPassword;
            }

            // Handle profile image upload (stored on disk, no DB schema changes).
            if (profileImage is not null && profileImage.Length > 0)
            {
                try
                {
                    // Allow some common formats used in mobile apps.
                    var allowedExt = new[] { ".png", ".jpg", ".jpeg", ".webp", ".gif", ".bmp" };
                    var ext = Path.GetExtension(profileImage.FileName)?.ToLowerInvariant() ?? "";

                    if (!allowedExt.Contains(ext))
                    {
                        ModelState.AddModelError("profileImage", "Invalid image type. Allowed: png/jpg/jpeg/webp/gif/bmp.");
                        ViewBag.ProfileImageUrl = GetProfileImageUrl(student.StudentId);
                        return View(model);
                    }

                    var dir = Path.Combine(_env.WebRootPath, "uploads", "profiles");
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    // Delete previous files for this student
                    var pattern = $"student_{student.StudentId}.*";
                    foreach (var oldPath in Directory.GetFiles(dir, pattern))
                    {
                        System.IO.File.Delete(oldPath);
                    }

                    var fileName = $"student_{student.StudentId}{ext}";
                    var targetPath = Path.Combine(dir, fileName);

                    using (var stream = new FileStream(targetPath, FileMode.Create))
                    {
                        profileImage.CopyTo(stream);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("profileImage", "Upload failed: " + ex.Message);
                    ViewBag.ProfileImageUrl = GetProfileImageUrl(student.StudentId);
                    return View(model);
                }
            }

            _context.SaveChanges();

            HttpContext.Session.SetString("StudentName", student.Name);
            HttpContext.Session.SetInt32("StudentId", student.StudentId);

            return RedirectToAction(nameof(Profile));
        }

        private string GetProfileImageUrl(int studentId)
        {
            var dir = Path.Combine(_env.WebRootPath, "uploads", "profiles");
            var pattern = $"student_{studentId}.*";

            try
            {
                if (Directory.Exists(dir))
                {
                    var files = Directory.GetFiles(dir, pattern);
                    var first = files.FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(first))
                    {
                        var fileName = Path.GetFileName(first);
                        return $"/uploads/profiles/{fileName}";
                    }
                }
            }
            catch
            {
                // ignore and fall back
            }

            return "/img/default-avatar.svg";
        }
    }
}
