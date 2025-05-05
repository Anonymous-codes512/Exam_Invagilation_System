using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Exam_Invagilation_System.Entities;  // Ensure Teacher model is included

namespace Exam_Invagilation_System.API
{
    [Route("api/teacher")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly AppDbContext _context;

        // Inject ApplicationDbContext to interact with the database
        public TeacherController(AppDbContext context)
        {
            _context = context;
        }

        // POST api/teacher/attendance_with_login
        [HttpPost("attendance_with_login")]
        public async Task<IActionResult> MarkAttendance([FromBody] TeacherAttendanceRequest request)
        {
            Console.WriteLine($"[TeacherController] Scanning teacher: {request.teacherEmployeeNumber}");

            if (string.IsNullOrEmpty(request.teacherEmployeeNumber))
            {
                return BadRequest(new { message = "Teacher Employee Number is required" });
            }

            // Query the database to find the teacher by TeacherEmployeeNumber
           var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.TeacherEmployeeNumber == request.teacherEmployeeNumber);
            if (teacher == null)
                return NotFound(new { message = "Teacher not found" });

            // Return the teacher details if found
            return Ok(new
            {
                teacherEmployeeNumber = teacher.TeacherEmployeeNumber,
                teacherName = teacher.TeacherName,
                teacherEmail = teacher.TeacherEmail,
                teacherDesignation = teacher.TeacherDesignation,
                teacherDepartment = teacher.TeacherDepartment,
                message = "Attendance marked successfully"
            });
        }
    }

    // Request Body Model
    public class TeacherAttendanceRequest
    {
        public string teacherEmployeeNumber { get; set; }
    }
}
