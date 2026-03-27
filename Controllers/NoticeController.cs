using Microsoft.AspNetCore.Mvc;
using Campus_Companion.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Campus_Companion.Controllers
{
    public class NoticeController : Controller
    {
        public static List<NoticeModel> Notices = new List<NoticeModel>()
        {
            new NoticeModel { Id = 1, Title = "Semester Exam Schedule", PublishedDate = DateTime.Now.AddDays(-2), TargetAudience = "All Students" },
            new NoticeModel { Id = 2, Title = "Faculty Meeting", PublishedDate = DateTime.Now.AddDays(-1), TargetAudience = "Teachers" }
        };

        public IActionResult Index()
        {
            return View(Notices);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(NoticeModel item)
        {
            item.Id = Notices.Any() ? Notices.Max(n => n.Id) + 1 : 1;
            item.PublishedDate = DateTime.Now;
            Notices.Add(item);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var item = Notices.FirstOrDefault(n => n.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        public IActionResult Edit(NoticeModel item)
        {
            var existing = Notices.FirstOrDefault(n => n.Id == item.Id);
            if (existing != null)
            {
                existing.Title = item.Title;
                existing.TargetAudience = item.TargetAudience;
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var item = Notices.FirstOrDefault(n => n.Id == id);
            if (item != null)
            {
                Notices.Remove(item);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
