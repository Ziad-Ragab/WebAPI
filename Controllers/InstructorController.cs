using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIDotNet.DTO;
using WebAPIDotNet.DTOS;
using WebAPIDotNet.Model;

namespace WebAPIDotNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class InstructorController : ControllerBase
    {
        private readonly ITIContext _context;

        public InstructorController(ITIContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var Instructors = await _context.Instructors
                .Include(i => i.Courses)
                .Select(i => new InstructorWithCoursesDTO
                {
                    Id = i.Id,
                    InstructorName = i.Name,
                    CourseNames = i.Courses.Select(c => c.Name).ToList()
                })
                .ToListAsync();

            if (Instructors.Count == 0)
                return NotFound(new GeneralResponse<string>
                {
                    Success = false,
                    Data = "Instructor not found"
                });

            return Ok(new GeneralResponse<List<InstructorWithCoursesDTO>>
            {
                Success = true,
                Data = Instructors
            });



        }



        [HttpGet("{id:int}")]

        public async Task<IActionResult> GetById(int id)
        {
            var instructor = await _context.Instructors
                .Include(I => I.Courses)
                .Where(i => i.Id == id)
                .Select(i => new InstructorWithCoursesDTO
                {
                    Id = i.Id,
                    InstructorName = i.Name,
                    CourseNames = i.Courses.Select(c => c.Name).ToList()
                })
                .FirstOrDefaultAsync();

            if (instructor == null)
                return NotFound(new GeneralResponse<string>
                {
                    Success = false,
                    Data = "Instructor not found"
                });

            return Ok(new GeneralResponse<InstructorWithCoursesDTO>
            {
                Success = true,
                Data = instructor
            });
        }

        [HttpPost("with-courses")]
        public async Task<IActionResult> CreateWithCourses([FromBody] InstructorCreateDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name) || dto.CourseIds == null || dto.CourseIds.Count == 0)
                return BadRequest(new GeneralResponse<string>
                {
                    Success = false,
                    Data = "Name and CourseIds are required"
                });

            var instructor = new Instructor { Name = dto.Name };

            var courses = await _context.Courses
                .Where(c => dto.CourseIds.Contains(c.Id))
                .ToListAsync();

            instructor.Courses = courses;

            await _context.Instructors.AddAsync(instructor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = instructor.Id },
                new GeneralResponse<string>
                {
                    Success = true,
                    Data = $"Instructor '{dto.Name}' created with {courses.Count} course(s)"
                });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] InstructorCreateDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name) || dto.CourseIds == null || dto.CourseIds.Count == 0)
                return BadRequest(new GeneralResponse<string>
                {
                    Success = false,
                    Data = "Name and CourseIds are required"
                });

            var instructor = await _context.Instructors.FindAsync(id);
            if (instructor == null)
                return NotFound(new GeneralResponse<string>
                {
                    Success = false,
                    Data = "Instructor not found"
                });
            instructor.Name = dto.Name;
            var courses = await _context.Courses
                .Where(c => dto.CourseIds.Contains(c.Id))
                .ToListAsync();

            instructor.Courses = courses;
            await _context.SaveChangesAsync();

            return Ok(new GeneralResponse<string>
            {
                Success = true,
                Data = "Instructor updated successfully"
            });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var courses = _context.Courses.Where(c => c.InstructorId == id);
            _context.Courses.RemoveRange(courses);
            var instructor = await _context.Instructors.FindAsync(id);
            if (instructor == null)
                return NotFound(new GeneralResponse<string>
                {
                    Success = false,
                    Data = "Instructor not found"
                });
            _context.Instructors.Remove(instructor);
            await _context.SaveChangesAsync();
            return Ok(new GeneralResponse<string>
            {
                Success = true,
                Data = "Instructor deleted successfully"
            });
        }
    }
}
