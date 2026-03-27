using Microsoft.AspNetCore.Mvc;
using Campus_Companion.Data;
using Campus_Companion.Models;
using Microsoft.EntityFrameworkCore;

namespace Campus_Companion.Controllers
{
    public class EventAdminController : Controller
    {
        private readonly AppDbContext _context;

        public EventAdminController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string? category, DateTime? fromDate, DateTime? toDate, string? subject, string? viewMode, DateTime? selectedDate)
        {
            var query = _context.Schedules.AsQueryable();

            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(e => e.Category == category);
            }

            if (fromDate.HasValue)
            {
                query = query.Where(e => e.Date.Date >= fromDate.Value.Date);
            }

            if (toDate.HasValue)
            {
                query = query.Where(e => e.Date.Date <= toDate.Value.Date);
            }

            if (!string.IsNullOrWhiteSpace(subject))
            {
                var s = subject.Trim();
                query = query.Where(e => e.Title.Contains(s));
            }

            var anchorDate = selectedDate?.Date ?? DateTime.Today;
            if (string.Equals(viewMode, "day", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(e => e.Date.Date == anchorDate);
            }
            else if (string.Equals(viewMode, "week", StringComparison.OrdinalIgnoreCase))
            {
                var weekStart = anchorDate.AddDays(-(int)anchorDate.DayOfWeek);
                var weekEnd = weekStart.AddDays(6);
                query = query.Where(e => e.Date.Date >= weekStart && e.Date.Date <= weekEnd);
            }

            var events = query
                .OrderBy(e => e.Date)
                .ThenBy(e => e.Time)
                .AsNoTracking()
                .ToList();

            ViewBag.SelectedDate = anchorDate.ToString("yyyy-MM-dd");
            ViewBag.ViewMode = string.IsNullOrWhiteSpace(viewMode) ? "week" : viewMode.ToLowerInvariant();

            return View(events);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new ScheduleItem { Date = DateTime.Today, Time = "09:00 AM" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ScheduleItem item)
        {
            if (string.IsNullOrWhiteSpace(item.Title))
            {
                ModelState.AddModelError(nameof(item.Title), "Title is required.");
            }

            if (item.Date.Date < DateTime.Today)
            {
                ModelState.AddModelError(nameof(item.Date), "Date cannot be in the past.");
            }

            if (!ModelState.IsValid)
            {
                return View(item);
            }

            _context.Schedules.Add(item);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var item = _context.Schedules.AsNoTracking().FirstOrDefault(e => e.ScheduleId == id);
            if (item is null)
            {
                return NotFound();
            }

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ScheduleItem item)
        {
            if (string.IsNullOrWhiteSpace(item.Title))
            {
                ModelState.AddModelError(nameof(item.Title), "Title is required.");
            }

            if (item.Date.Date < DateTime.Today)
            {
                ModelState.AddModelError(nameof(item.Date), "Date cannot be in the past.");
            }

            if (!ModelState.IsValid)
            {
                return View(item);
            }

            var existing = _context.Schedules.FirstOrDefault(e => e.ScheduleId == item.ScheduleId);
            if (existing is null)
            {
                return NotFound();
            }

            existing.Title = item.Title;
            existing.Time = item.Time;
            existing.Location = item.Location;
            existing.Category = item.Category;
            existing.Date = item.Date;

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var item = _context.Schedules.FirstOrDefault(e => e.ScheduleId == id);
            if (item is null)
            {
                return NotFound();
            }

            _context.Schedules.Remove(item);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
