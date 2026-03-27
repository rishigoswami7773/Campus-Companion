using Microsoft.AspNetCore.Mvc;
using Campus_Companion.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Campus_Companion.Controllers
{
    public class EventAdminController : Controller
    {
        public static List<EventModel> Events = new List<EventModel>()
        {
            new EventModel { Id = 1, Name = "Tech Fest 2026", EventDate = DateTime.Now.AddDays(15), Location = "Main Auditorium", Status = "Upcoming" },
            new EventModel { Id = 2, Name = "Career Fair", EventDate = DateTime.Now.AddDays(22), Location = "Campus Ground", Status = "Upcoming" }
        };

        public IActionResult Index()
        {
            return View(Events);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(EventModel item)
        {
            item.Id = Events.Any() ? Events.Max(e => e.Id) + 1 : 1;
            if (string.IsNullOrEmpty(item.Status)) item.Status = "Upcoming";
            Events.Add(item);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var item = Events.FirstOrDefault(e => e.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        public IActionResult Edit(EventModel item)
        {
            var existing = Events.FirstOrDefault(e => e.Id == item.Id);
            if (existing != null)
            {
                existing.Name = item.Name;
                existing.EventDate = item.EventDate;
                existing.Location = item.Location;
                existing.Status = item.Status;
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var item = Events.FirstOrDefault(e => e.Id == id);
            if (item != null)
            {
                Events.Remove(item);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
