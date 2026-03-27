using Microsoft.AspNetCore.Mvc;
using Campus_Companion.Data;
using Campus_Companion.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Campus_Companion.Controllers
{
    public class StudentTaskController : Controller
    {
        private readonly AppDbContext _context;

        public StudentTaskController(AppDbContext context)
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

            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "tasks-student-export.csv");
        }
    }
}
