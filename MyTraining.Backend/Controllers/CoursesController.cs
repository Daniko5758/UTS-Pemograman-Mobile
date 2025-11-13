using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyTraining.Backend.Data;
using MyTraining.Backend.Models;
using Microsoft.AspNetCore.Hosting; 
using System.IO;                   

namespace MyTraining.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public CoursesController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

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

            if (course.TrainerId == 0 && course.Trainer != null)
            {
                course.TrainerId = course.Trainer.Id;
            }
            course.Trainer = null; 

            _db.Entry(course).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_db.Courses.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
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

        // --- METODE UPLOAD ANDA (SUDAH BENAR) ---
        [HttpPost("upload")]
        public async Task<IActionResult> UploadCoverImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var uploadPath = Path.Combine(_env.ContentRootPath, "wwwroot", "images", "courses");
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            var fileExtension = Path.GetExtension(file.FileName);
            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(uploadPath, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var webAccessiblePath = $"/images/courses/{uniqueFileName}";
            return Ok(new { FilePath = webAccessiblePath });
        }
    }
}