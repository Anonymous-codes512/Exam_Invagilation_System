using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Exam_Invagilation_System.Entities;
using Exam_Invagilation_System.Models;  // Ensure Teacher model is included

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

        [HttpPost("unfair-means-report")]
        public async Task<IActionResult> SubmitUnfairMeansReport([FromBody] UnfairMeansReport report)
        {
            try
            {
                // Validation for the incoming report
                if (report == null)
                {
                    return BadRequest(new { message = "Invalid report data." });
                }
                
                DateOnly dateOnly = DateOnly.FromDateTime(report.date); // Convert the DateTime to DateOnly

                // Map UnfairMeansReport to CheatingReport
                var cheatingReport = new CheatingReport
                {
                    RegistrationNumber = report.studentRegistration,
                    TeacherEmployeeNumber = report.teacherEmployeeNumber,
                    CourseCode = report.courseCode,
                    Date = dateOnly, // Assign the DateOnly value
                    TimeSlot = report.date.ToString("hh:mm tt"), // Example: Convert date to time slot
                    UnfairType = report.unfairType,
                    OtherDetails = report.otherDetails,
                    IncidentDetails = report.incidentDetails,
                    RoomNumber = report.roomNumber
                };

                // Check if the Student, Teacher, Room, and Course exist
                var student = await _context.Students.FirstOrDefaultAsync(s => s.RegistrationNumber == report.studentRegistration);
                var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.TeacherEmployeeNumber == report.teacherEmployeeNumber);
                var room = await _context.Rooms.FirstOrDefaultAsync(r => r.RoomNumber == report.roomNumber);
                var course = await _context.Courses.FirstOrDefaultAsync(c => c.CourseCode == report.courseCode);

                if (student == null || teacher == null || room == null || course == null)
                {
                    return BadRequest(new { message = "Invalid student, teacher, room, or course information." });
                }

                // Set the related entities
                cheatingReport.Student = student;
                cheatingReport.Teacher = teacher;
                cheatingReport.Room = room;
                cheatingReport.Course = course;

                // Save the report to the database
                await _context.CheatingReports.AddAsync(cheatingReport);
                await _context.SaveChangesAsync();

                // Return a success response
                return Ok(new { success = true, message = "Report submitted successfully!" });
            }
            catch (Exception ex)
            {
                // Log exception and return an error response
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, new { success = false, message = "Failed to submit the report.", error = ex.Message });
            }
        }


        [HttpPost("paper-collection")]
        public async Task<IActionResult> SubmitPaperCollection([FromBody] PaperCollection report)
        {
            Console.WriteLine("Processing submit paper collection... ");
            try
            {
                // Validate the report data
                if (report == null)
                {
                    return BadRequest(new { message = "Invalid report data." });
                }

                // Validate if the teacher, course, and room exist
                var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.TeacherEmployeeNumber == report.teacherNumber);
                var course = await _context.Courses.FirstOrDefaultAsync(c => c.CourseCode == report.selectedPaper);
                var room = await _context.Rooms.FirstOrDefaultAsync(r => r.RoomNumber == report.roomNumber);

                if (teacher == null || course == null || room == null)
                {
                    return BadRequest(new { message = "Invalid teacher, course, or room." });
                }

                // Map report data to PaperCollection model
                var paperCollection = new PaperSummaryCollection
                {
                    TeacherEmployeeNumber = report.teacherNumber,
                    SelectedPaper = report.selectedPaper,
                    roomnumber = report.roomNumber,
                    TotalPapers = report.totalPapers,
                    NumberOfPapers = report.numberOfPapers,
                    CollectorName = report.collectorName,
                    CollectedDate = DateTime.Now
                };

                // Save the paper collection data to the database
                await _context.PaperSummaryCollections.AddAsync(paperCollection);
                await _context.SaveChangesAsync();

                // Return success response
                return Ok(new { success = true, message = "Paper collection report submitted successfully!" });
            }
            catch (Exception ex)
            {
                // Handle error (log the exception)
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, new { success = false, message = "Failed to submit the report.", error = ex.Message });
            }
        }

    }



    // Request Body Model
    public class TeacherAttendanceRequest
    {
        public string teacherEmployeeNumber { get; set; }
    }

    public class UnfairMeansReport
    {
        public string studentRegistration { get; set; }
        public string teacherEmployeeNumber { get; set; }
        public string courseCode { get; set; }
        public DateTime date { get; set; }
        public string unfairType { get; set; }
        public string otherDetails { get; set; }
        public string incidentDetails { get; set; }
        public string roomNumber { get; set; }
    }

    public class PaperCollection
    {
        public string teacherNumber {get; set;}
        public string selectedPaper {get; set;}
        public int totalPapers { get; set; }
        public int numberOfPapers { get; set; }
        public string collectorName { get; set; }
        public string roomNumber { get; set; }
    }
}
