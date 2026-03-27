using Microsoft.AspNetCore.Mvc;
using Campus_Companion.Data;
using Campus_Companion.Models;
using Microsoft.EntityFrameworkCore;

namespace Campus_Companion.Controllers
{
    public class DashboardAdminController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardAdminController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var today = DateTime.Today;
            var totalTasks = _context.Tasks.Count();
            var completedTasks = _context.Tasks.Count(t => t.Status == ItemStatus.Completed);

            var urgentTaskCount = _context.Tasks.Count(t =>
                t.Status == ItemStatus.Pending &&
                t.DueDate.Date >= today &&
                t.DueDate.Date < today.AddDays(2));

            var completedPercentage = totalTasks == 0
                ? 0
                : (double)completedTasks * 100.0 / totalTasks;

            var pendingTasks = _context.Tasks.Where(t => t.Status == ItemStatus.Pending);
            var dueTomorrowCount = _context.Tasks.Count(t => t.Status == ItemStatus.Pending && t.DueDate.Date == today.AddDays(1));
            var smartSuggestions = new List<string>();
            if (urgentTaskCount > 0)
            {
                smartSuggestions.Add($"You have {urgentTaskCount} urgent task(s) today.");
            }
            if (dueTomorrowCount > 0)
            {
                smartSuggestions.Add($"{dueTomorrowCount} assignment(s) due tomorrow.");
            }
            if (_context.Tasks.Any(t => t.Status == ItemStatus.Pending && t.Title.Contains("DBMS")))
            {
                smartSuggestions.Add("Focus on DBMS assignment first.");
            }
            if (smartSuggestions.Count == 0)
            {
                smartSuggestions.Add("Great pace! Keep tracking your schedule daily.");
            }

            var vm = new DashboardViewModel
            {
                TotalTasks = totalTasks,
                CompletedTasks = completedTasks,
                CompletedPercentage = completedPercentage,
                UrgentTaskCount = urgentTaskCount,
                PendingHighCount = pendingTasks.Count(t => t.Priority == TaskPriority.High),
                PendingMediumCount = pendingTasks.Count(t => t.Priority == TaskPriority.Medium),
                PendingLowCount = pendingTasks.Count(t => t.Priority == TaskPriority.Low),
                DueTomorrowCount = dueTomorrowCount,
                UpcomingEvents = _context.Schedules.Count(e => e.Date >= today),
                TotalNotices = _context.Notices.Count(),
                SmartSuggestions = smartSuggestions,
                PriorityTasks = _context.Tasks
                    .Where(t => t.Status == ItemStatus.Pending)
                    .OrderByDescending(t => t.Priority)
                    .ThenBy(t => t.DueDate)
                    .Take(5)
                    .AsNoTracking()
                    .ToList(),
                UpcomingDeadlines = _context.Tasks
                    .Where(t => t.Status == ItemStatus.Pending && t.DueDate.Date >= today)
                    .OrderBy(t => t.DueDate)
                    .Take(5)
                    .AsNoTracking()
                    .ToList(),
                TodaySchedule = _context.Schedules
                    .Where(s => s.Date == today)
                    .OrderBy(s => s.Time)
                    .Take(5)
                    .AsNoTracking()
                    .ToList(),
                UpcomingSchedule = _context.Schedules
                    .Where(s => s.Date >= today)
                    .OrderBy(s => s.Date)
                    .ThenBy(s => s.Time)
                    .Take(5)
                    .AsNoTracking()
                    .ToList(),
                LatestNotices = _context.Notices
                    .Where(n => n.Category == "Exam" || n.Category == "Important" || n.Category == "Exam Deadline")
                    .OrderByDescending(n => n.Date)
                    .Take(5)
                    .AsNoTracking()
                    .ToList()
            };

            return View(vm);
        }
    }
}
