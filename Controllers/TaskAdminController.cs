using Microsoft.AspNetCore.Mvc;
using Campus_Companion.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Campus_Companion.Controllers
{
    public class TaskAdminController : Controller
    {
        public static List<TaskItem> Tasks = new List<TaskItem>()
        {
            new TaskItem { Id = 1, Title = "Finish MVC Project", DueDate = DateTime.Now.AddDays(2), Priority = "High" },
            new TaskItem { Id = 2, Title = "Read Bootstrap Docs", DueDate = DateTime.Now.AddDays(5), Priority = "Medium" },
            new TaskItem { Id = 3, Title = "Prepare Database", DueDate = DateTime.Now.AddDays(7), Priority = "Low" }
        };

        public IActionResult Index()
        {
            return View(Tasks);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(TaskItem item)
        {
            item.Id = Tasks.Any() ? Tasks.Max(t => t.Id) + 1 : 1;
            Tasks.Add(item);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var item = Tasks.FirstOrDefault(t => t.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        public IActionResult Edit(TaskItem item)
        {
            var existing = Tasks.FirstOrDefault(t => t.Id == item.Id);
            if (existing != null)
            {
                existing.Title = item.Title;
                existing.DueDate = item.DueDate;
                existing.Priority = item.Priority;
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var item = Tasks.FirstOrDefault(t => t.Id == id);
            if (item != null)
            {
                Tasks.Remove(item);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
