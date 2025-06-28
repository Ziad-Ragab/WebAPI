using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIDotNet.DTO;
using WebAPIDotNet.DTOS;
using WebAPIDotNet.Model;

namespace WebAPIDotNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CourseController : ControllerBase
    {
        private readonly ITIContext _context;

        public CourseController(ITIContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.Courses
                .Select(c => new CourseDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    InstructorName = c.Instructor.Name
                })
                .ToListAsync();

            return Ok(new GeneralResponse<List<CourseDTO>>
            {
                Success = true,
                Data = data
            });
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Instructor)
                .Where(c => c.Id == id)
                .Select(c => new CourseDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    InstructorName = c.Instructor.Name
                })
                .FirstOrDefaultAsync();

            if (course == null)
                return NotFound(new GeneralResponse<string>
                {
                    Success = false,
                    Data = "Course not found"
                });

            return Ok(new GeneralResponse<CourseDTO>
            {
                Success = true,
                Data = course
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(CourseCreateDTO dto)
        {
            var course = new Course
            {
                Name = dto.Name,
                InstructorId = dto.InstructorId
            };

            await _context.Courses.AddAsync(course);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = course.Id },
                new GeneralResponse<Course>
                {
                    Success = true,
                    Data = course
                });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CourseCreateDTO dto)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
                return NotFound(new GeneralResponse<string>
                {
                    Success = false,
                    Data = "Course not found"
                });

            course.Name = dto.Name;
            course.InstructorId = dto.InstructorId;

            await _context.SaveChangesAsync();

            return Ok(new GeneralResponse<string>
            {
                Success = true,
                Data = "Course updated successfully"
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
                return NotFound(new GeneralResponse<string>
                {
                    Success = false,
                    Data = "Course not found"
                });

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return Ok(new GeneralResponse<string>
            {
                Success = true,
                Data = "Course deleted successfully"
            });
        }







    }
}
