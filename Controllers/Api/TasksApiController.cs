using Campus_Companion.Data;
using Campus_Companion.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Campus_Companion.Controllers.Api
{
    [Route("api/tasks")]
    [ApiController]
    public class TasksApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TasksApiController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_context.Tasks.AsNoTracking().OrderBy(t => t.DueDate).ToList());

        [HttpPost]
        public IActionResult Create(TaskItem task)
        {
            if (string.IsNullOrWhiteSpace(task.Title))
            {
                return BadRequest("Task title is required.");
            }

            task.Priority = (task.DueDate.Date - DateTime.Today).TotalDays <= 2 ? TaskPriority.High : task.Priority;
            _context.Tasks.Add(task);
            _context.SaveChanges();
            return Ok(task);
        }

        [HttpPut("{id:int}/toggle")]
        public IActionResult ToggleStatus(int id)
        {
            var task = _context.Tasks.FirstOrDefault(t => t.TaskId == id);
            if (task is null) return NotFound();
            task.Status = task.Status == ItemStatus.Completed ? ItemStatus.Pending : ItemStatus.Completed;
            _context.SaveChanges();
            return Ok(task);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var task = _context.Tasks.FirstOrDefault(t => t.TaskId == id);
            if (task is null) return NotFound();
            _context.Tasks.Remove(task);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
