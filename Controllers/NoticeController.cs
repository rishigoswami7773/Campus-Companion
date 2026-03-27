using Microsoft.AspNetCore.Mvc;
using Campus_Companion.Data;
using Campus_Companion.Models;
using Microsoft.EntityFrameworkCore;

namespace Campus_Companion.Controllers
{
    public class NoticeController : Controller
    {
        private readonly AppDbContext _context;

        public NoticeController(AppDbContext context)
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

        [HttpGet]
        public IActionResult Create()
        {
            return View(new NoticeItem { Date = DateTime.Today });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(NoticeItem notice)
        {
            if (string.IsNullOrWhiteSpace(notice.Title))
            {
                ModelState.AddModelError(nameof(notice.Title), "Title is required.");
            }

            if (!ModelState.IsValid)
            {
                return View(notice);
            }

            _context.Notices.Add(notice);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var notice = _context.Notices.AsNoTracking().FirstOrDefault(n => n.NoticeId == id);
            if (notice is null)
            {
                return NotFound();
            }

            return View(notice);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(NoticeItem notice)
        {
            if (string.IsNullOrWhiteSpace(notice.Title))
            {
                ModelState.AddModelError(nameof(notice.Title), "Title is required.");
            }

            if (!ModelState.IsValid)
            {
                return View(notice);
            }

            var existing = _context.Notices.FirstOrDefault(n => n.NoticeId == notice.NoticeId);
            if (existing is null)
            {
                return NotFound();
            }

            existing.Title = notice.Title;
            existing.Content = notice.Content;
            existing.Date = notice.Date;
            existing.Category = notice.Category;

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var notice = _context.Notices.FirstOrDefault(n => n.NoticeId == id);
            if (notice is null)
            {
                return NotFound();
            }

            _context.Notices.Remove(notice);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
