using Exam_Invagilation_System.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Exam_Invagilation_System.API
{
    [Route("api/student")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly AppDbContext _context;
        public StudentController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("attendance_by_qr_code")]
        public async Task<IActionResult> MarkStudentAttendance([FromBody] StudentAttendanceRequest request)
        {
            Console.WriteLine("....");

            // Extract incoming data
            DateTime fullDateTime = request.DateAndTime;
            string dateString = "2025/04/20"; // The date string
            DateOnly dateOnly = DateOnly.Parse(dateString); // Parsing the string into DateOnly
            string timeSlot = "1:00 PM - 2:30 PM"; // Hardcoded time slot (can be dynamic as needed)

            string studentRegistrationNumber = request.StudentRegistrationNumber;
            string teacherEmployeeNumber = request.TeacherEmployeeNumber;

            if (string.IsNullOrEmpty(studentRegistrationNumber) && string.IsNullOrEmpty(teacherEmployeeNumber))
            {
                return BadRequest(new { message = "Student Registration Number is required" });
            }

            // Query to get the paper and course attendance details
            var result = await _context.Papers
                .Where(p => p.Date == dateOnly && p.TimeSlot == timeSlot)
                .Join(_context.StudentCourses,
                    paper => paper.CourseCode,
                    sc => sc.CourseCode,
                    (paper, sc) => new { paper, sc })
                .Where(joined => joined.sc.RegistrationNumber == studentRegistrationNumber)
                .Select(joined => new
                {
                    StudentRegistrationNumber = studentRegistrationNumber,
                    TeacherEmployeeNumber = teacherEmployeeNumber,
                    joined.paper.Room.RoomNumber,
                    joined.paper.PaperId,
                    joined.paper.Date,
                    joined.paper.CourseCode,
                    CourseName = joined.paper.Course.CourseName,
                    joined.paper.TimeSlot,
                    joined.paper.ExamTerm,
                    CourseAttendance = joined.sc.CourseAttendance // Retrieve the string value of CourseAttendance
                })
                .FirstOrDefaultAsync();

            // Log result for debugging
            Console.WriteLine(result);

            // If no results found or attendance percentage is below 80%
            if (result == null)
            {
                return NotFound(new { message = "No Student found for the given date and time." });
            }

            // Check if the attendance percentage is greater than or equal to 80%
            double CourseAttendance;
            bool isValid = double.TryParse(result.CourseAttendance, out CourseAttendance);

            if (isValid && CourseAttendance >= 80)
            {
                // Insert attendance record into Attendance table
                var attendance = new Attendance
                {
                    RegistrationNumber = studentRegistrationNumber,
                    TeacherEmployeeNumber = teacherEmployeeNumber,
                    RoomNumber = result.RoomNumber,
                    PaperId = result.PaperId,
                    Date = result.Date,
                    TimeSlot = result.TimeSlot,
                    Status = "Present" // Assuming status is "Present", update as required
                };

                // Insert attendance into database
                _context.Attendances.Add(attendance);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Attendance marked successfully",
                    student = studentRegistrationNumber,
                    time = timeSlot
                });
            }

            return BadRequest(new { message = "Attendance percentage is below 80%" });
        }

        [HttpGet("attendance_by_room")]
        public async Task<IActionResult> GetAttendanceByRoom([FromQuery] string teacherEmployeeNumber, [FromQuery] string dateAndTime)
        {
            Console.WriteLine(teacherEmployeeNumber, dateAndTime);
            try
            {
                // Parse the date and time string to DateTime
                DateTime parsedDateTime = DateTime.Parse(dateAndTime);

                //// Query the database to get student attendance data based on teacher and date/time
                //var students = await _context.StudentCourses
                //    .Where(sc => sc.TeacherEmployeeNumber == teacherEmployeeNumber)
                //    .Join(_context.Papers,
                //        sc => sc.CourseCode,
                //        p => p.CourseCode,
                //        (sc, p) => new { sc, p })
                //    .Where(joined => joined.p.Date == parsedDateTime.Date)
                //    .Select(joined => new
                //    {
                //        joined.sc.Name,  // Assuming there's a Name field in StudentCourses
                //        joined.sc.SeatNo,
                //        joined.sc.RegNo,
                //        joined.p.PaperName,  // Assuming PaperName field exists in Papers
                //        joined.p.TimeSlot,
                //        AttendanceStatus = "Absent"  // Default status, can be changed dynamically
                //    })
                //    .ToListAsync();

                // If no students found, return a not found status
                //if (!students.Any())
                //{
                //    return NotFound(new { message = "No student attendance data found for the given teacher and date." });
                //}

                // Return the list of students as JSON
                return Ok(new { success = true, data = "students" });
            }
            catch (Exception ex)
            {
                // Return error if something goes wrong
                return StatusCode(500, new { success = false, message = "An error occurred: " + ex.Message });
            }
        }



    }
    public class StudentAttendanceRequest
    {
        public required string StudentRegistrationNumber { get; set; }
        public required string TeacherEmployeeNumber { get; set; }

        public required DateTime DateAndTime { get; set;}
    }
}
