using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyTraining.Backend.Data;
using MyTraining.Backend.Models;

namespace MyTraining.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly AppDbContext _db;
        public CoursesController(AppDbContext db) => _db = db;

        [HttpGet]
        public async Task<ActionResult<List<Course>>> Get([FromQuery] string? search)
        {
            var q = _db.Courses.Include(c => c.Trainer).AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
                q = q.Where(c => c.Title.Contains(search) || c.Description.Contains(search));
            return await q.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetById(int id)
        {
            var c = await _db.Courses.Include(x => x.Trainer).FirstOrDefaultAsync(x => x.Id == id);
            if (c == null) return NotFound();
            return c;
        }

        [HttpPost]
        public async Task<ActionResult<Course>> Create(Course course)
        {
            // if client passed nested Trainer object, set TrainerId and null the nav prop
            if (course.Trainer != null)
            {
                course.TrainerId = course.Trainer.Id;
                course.Trainer = null;
            }
            _db.Courses.Add(course);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = course.Id }, course);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Course course)
        {
            if (id != course.Id) return BadRequest();
            _db.Entry(course).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var c = await _db.Courses.FindAsync(id);
            if (c == null) return NotFound();
            _db.Courses.Remove(c);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
