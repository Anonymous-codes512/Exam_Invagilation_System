using Exam_Invagilation_System.Entities;
using Microsoft.AspNetCore.Mvc;

namespace YourProjectNamespace.Api
{
    [Route("api/teacher")]
    [ApiController]
    public class TeacherController : Controller
    {
        private readonly AppDbContext _context;

        public TeacherController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("attendance_with_login")]
        public IActionResult MarkAttendance([FromBody] string teacherEmployeeNumber)
        {
            if (string.IsNullOrEmpty(teacherEmployeeNumber))
                return BadRequest("Teacher Employee Number is required");

            var teacher = _context.Teachers
                .FirstOrDefault(t => t.TeacherEmployeeNumber == teacherEmployeeNumber);

            if (teacher == null)
                return NotFound("Teacher not found");

            Console.WriteLine($"✅ Teacher Found: {teacher.TeacherName}");

            return Ok(new
            {
                message = "Attendance marked successfully",
                teacherName = teacher.TeacherName,
                teacherEmail = teacher.TeacherEmail
            });
        }

    }
}
