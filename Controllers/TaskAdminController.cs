using Microsoft.AspNetCore.Mvc;
using Campus_Companion.Data;
using Campus_Companion.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Campus_Companion.Controllers
{
    public class TaskAdminController : Controller
    {
        private readonly AppDbContext _context;

        public TaskAdminController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string? priority, string? status, DateTime? fromDate, DateTime? toDate, string? search)
        {
            var query = _context.Tasks.AsQueryable();

            if (Enum.TryParse<TaskPriority>(priority, true, out var selectedPriority))
            {
                query = query.Where(t => t.Priority == selectedPriority);
            }

            if (Enum.TryParse<ItemStatus>(status, true, out var selectedStatus))
            {
                query = query.Where(t => t.Status == selectedStatus);
            }

            if (fromDate.HasValue)
            {
                query = query.Where(t => t.DueDate.Date >= fromDate.Value.Date);
            }

            if (toDate.HasValue)
            {
                query = query.Where(t => t.DueDate.Date <= toDate.Value.Date);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                var text = search.Trim();
                query = query.Where(t => t.Title.Contains(text) || t.Description.Contains(text));
            }

            var tasks = query
                .OrderBy(t => t.Status)
                .ThenBy(t => t.DueDate)
                .AsNoTracking()
                .ToList();

            return View(tasks);
        }

        [HttpGet]
        public IActionResult ExportCsv()
        {
            var tasks = _context.Tasks
                .OrderBy(t => t.DueDate)
                .AsNoTracking()
                .ToList();

            var sb = new StringBuilder();
            sb.AppendLine("Title,Description,DueDate,Priority,Status");
            foreach (var item in tasks)
            {
                sb.AppendLine($"\"{item.Title.Replace("\"", "\"\"")}\",\"{item.Description.Replace("\"", "\"\"")}\",{item.DueDate:yyyy-MM-dd},{item.Priority},{item.Status}");
            }

            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "tasks-backup.csv");
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new TaskItem { DueDate = DateTime.Today.AddDays(1) });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TaskItem task)
        {
            if (string.IsNullOrWhiteSpace(task.Title))
            {
                ModelState.AddModelError(nameof(task.Title), "Task title is required.");
            }

            if (task.DueDate.Date < DateTime.Today)
            {
                ModelState.AddModelError(nameof(task.DueDate), "Date cannot be in the past.");
            }

            if (!ModelState.IsValid)
            {
                return View(task);
            }

            task.Priority = AutoSetPriority(task.DueDate, task.Priority);
            _context.Tasks.Add(task);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var task = _context.Tasks.AsNoTracking().FirstOrDefault(t => t.TaskId == id);
            if (task is null)
            {
                return NotFound();
            }

            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TaskItem task)
        {
            if (string.IsNullOrWhiteSpace(task.Title))
            {
                ModelState.AddModelError(nameof(task.Title), "Task title is required.");
            }

            if (task.DueDate.Date < DateTime.Today)
            {
                ModelState.AddModelError(nameof(task.DueDate), "Date cannot be in the past.");
            }

            if (!ModelState.IsValid)
            {
                return View(task);
            }

            var existing = _context.Tasks.FirstOrDefault(t => t.TaskId == task.TaskId);
            if (existing is null)
            {
                return NotFound();
            }

            existing.Title = task.Title;
            existing.Description = task.Description;
            existing.DueDate = task.DueDate;
            existing.Priority = AutoSetPriority(task.DueDate, task.Priority);

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleStatus(int id)
        {
            var task = _context.Tasks.FirstOrDefault(t => t.TaskId == id);
            if (task is null)
            {
                return NotFound();
            }

            task.Status = task.Status == ItemStatus.Completed ? ItemStatus.Pending : ItemStatus.Completed;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var task = _context.Tasks.FirstOrDefault(t => t.TaskId == id);
            if (task is null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private static TaskPriority AutoSetPriority(DateTime dueDate, TaskPriority selectedPriority)
        {
            // Smart feature: due date within next 2 days -> High priority
            return dueDate.Date < DateTime.Today.AddDays(2) ? TaskPriority.High : selectedPriority;
        }
    }
}
