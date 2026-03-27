using Campus_Companion.Data;
using Campus_Companion.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Campus_Companion.Controllers.Api
{
    [Route("api/notices")]
    [ApiController]
    public class NoticesApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public NoticesApiController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_context.Notices.AsNoTracking().OrderByDescending(n => n.Date).ToList());

        [HttpPost]
        public IActionResult Create(NoticeItem notice)
        {
            if (string.IsNullOrWhiteSpace(notice.Title)) return BadRequest("Title is required.");
            _context.Notices.Add(notice);
            _context.SaveChanges();
            return Ok(notice);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var notice = _context.Notices.FirstOrDefault(n => n.NoticeId == id);
            if (notice is null) return NotFound();
            _context.Notices.Remove(notice);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
