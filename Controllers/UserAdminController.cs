using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Campus_Companion.Models;

namespace Campus_Companion.Controllers
{
    public class UserAdminController : Controller
    {
        public static List<AppUser> Users = new List<AppUser>()
        {
            new AppUser { Id = 1, Name = "Rishi", Role = "Student", Email = "rishi@gmail.com", Status = "Active" },
            new AppUser { Id = 2, Name = "Harsh", Role = "Teacher", Email = "harsh@gmail.com", Status = "Active" }
        };

        public IActionResult Index()
        {
            return View(Users);
        }

        public IActionResult CreateStudent()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateStudent(AppUser user)
        {
            user.Id = Users.Any() ? Users.Max(u => u.Id) + 1 : 1;
            user.Role = "Student";
            user.Status = "Active";
            Users.Add(user);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult CreateTeacher()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateTeacher(AppUser user)
        {
            user.Id = Users.Any() ? Users.Max(u => u.Id) + 1 : 1;
            user.Role = "Teacher";
            user.Status = "Active";
            Users.Add(user);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var user = Users.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost]
        public IActionResult Edit(AppUser user)
        {
            var existing = Users.FirstOrDefault(u => u.Id == user.Id);
            if (existing != null)
            {
                existing.Name = user.Name;
                existing.Email = user.Email;
                existing.Status = user.Status;
                // Don't change Role as it's set by the sub-forms
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var user = Users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                Users.Remove(user);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
