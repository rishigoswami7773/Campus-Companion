using Microsoft.AspNetCore.Mvc;
using Campus_Companion.Data;
using Campus_Companion.Models;
using Microsoft.EntityFrameworkCore;

namespace Campus_Companion.Controllers
{
    public class StudentEventController : Controller
    {
        private readonly AppDbContext _context;

        public StudentEventController(AppDbContext context)
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
    }
}
