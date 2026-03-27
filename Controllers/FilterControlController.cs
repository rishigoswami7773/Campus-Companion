using Microsoft.AspNetCore.Mvc;
using Campus_Companion.Models;
using System.Collections.Generic;
using System.Linq;

namespace Campus_Companion.Controllers
{
    public class FilterControlController : Controller
    {
        public static List<FilterRule> Rules = new List<FilterRule>()
        {
            new FilterRule { Id = 1, Name = "Block Profanity", Category = "Content", Status = "Active" }
        };

        public IActionResult Index()
        {
            return View(Rules);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(FilterRule rule)
        {
            rule.Id = Rules.Any() ? Rules.Max(r => r.Id) + 1 : 1;
            Rules.Add(rule);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var rule = Rules.FirstOrDefault(r => r.Id == id);
            if (rule == null) return NotFound();
            return View(rule);
        }

        [HttpPost]
        public IActionResult Edit(FilterRule rule)
        {
            var existing = Rules.FirstOrDefault(r => r.Id == rule.Id);
            if (existing != null)
            {
                existing.Name = rule.Name;
                existing.Category = rule.Category;
                existing.Status = rule.Status;
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var rule = Rules.FirstOrDefault(r => r.Id == id);
            if (rule != null)
            {
                Rules.Remove(rule);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
