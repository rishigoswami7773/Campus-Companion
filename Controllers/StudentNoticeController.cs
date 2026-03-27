using Microsoft.AspNetCore.Mvc;
using Campus_Companion.Data;
using Campus_Companion.Models;
using Microsoft.EntityFrameworkCore;

namespace Campus_Companion.Controllers
{
    public class StudentNoticeController : Controller
    {
        private readonly AppDbContext _context;

        public StudentNoticeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string? category, DateTime? fromDate, DateTime? toDate)
        {
            var query = _context.Notices.AsQueryable();
            if (!string.IsNullOrWhiteSpace(category))
            {
                var c = category.Trim().ToLower();
                if (c == "important")
                {
                    query = query.Where(n =>
                        n.Category.ToLower().Contains("important") ||
                        n.Category.ToLower().Contains("exam") ||
                        n.Category.ToLower().Contains("urgent"));
                }
                else if (c == "normal")
                {
                    query = query.Where(n =>
                        !n.Category.ToLower().Contains("important") &&
                        !n.Category.ToLower().Contains("exam") &&
                        !n.Category.ToLower().Contains("urgent"));
                }
                else
                {
                    query = query.Where(n => n.Category == category);
                }
            }

            if (fromDate.HasValue)
            {
                query = query.Where(n => n.Date.Date >= fromDate.Value.Date);
            }

            if (toDate.HasValue)
            {
                query = query.Where(n => n.Date.Date <= toDate.Value.Date);
            }

            var notices = query
                .OrderByDescending(n => n.Date)
                .AsNoTracking()
                .ToList();

            return View(notices);
        }
    }
}
