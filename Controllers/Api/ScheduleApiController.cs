using Campus_Companion.Data;
using Campus_Companion.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Campus_Companion.Controllers.Api
{
    [Route("api/schedule")]
    [ApiController]
    public class ScheduleApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ScheduleApiController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_context.Schedules.AsNoTracking().OrderBy(s => s.Date).ToList());

        [HttpPost]
        public IActionResult Create(ScheduleItem item)
        {
            if (string.IsNullOrWhiteSpace(item.Title)) return BadRequest("Title is required.");
            _context.Schedules.Add(item);
            _context.SaveChanges();
            return Ok(item);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var item = _context.Schedules.FirstOrDefault(s => s.ScheduleId == id);
            if (item is null) return NotFound();
            _context.Schedules.Remove(item);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
