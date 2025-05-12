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

            string studentRegistrationNumber = request.RegistrationNumber;
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

        [HttpGet("fetch_student_from_room")]
        public async Task<IActionResult> fetchStudentsFromRoom([FromQuery] string teacherEmployeeNumber, [FromQuery] string dateAndTime)
        {
            Console.WriteLine($"Teacher: {teacherEmployeeNumber}, Date and Time: {dateAndTime}");

            try
            {
                // 🔧 Parse the input DateTime
                DateTime parsedDateTime = DateTime.Parse(dateAndTime);
                DateOnly dateOnly = DateOnly.FromDateTime(parsedDateTime);
                TimeSpan currentTime = parsedDateTime.TimeOfDay;

                // 🔧 Get all duties for the teacher on that date
                var allDuties = await _context.Duties
                    .Include(d => d.Room)
                    .Join(_context.Papers, d => d.PaperId, p => p.PaperId, (d, p) => new { d, p })
                    .Join(_context.Teachers, dp => dp.d.TeacherId, t => t.TeacherId, (dp, t) => new { dp.d, dp.p, t })
                    .Where(x => x.t.TeacherEmployeeNumber == teacherEmployeeNumber && x.p.Date == dateOnly)
                    .ToListAsync(); // 🔧 bring to memory for TimeSlot parsing

                // 🔧 Find the duty where the current time falls into the TimeSlot range
                var duty = allDuties.FirstOrDefault(x =>
                {
                    var parts = x.p.TimeSlot.Split(" - ");
                    var startTime = DateTime.Parse(parts[0]).TimeOfDay;
                    var endTime = DateTime.Parse(parts[1]).TimeOfDay;
                    return currentTime >= startTime && currentTime <= endTime;
                });

                if (duty == null)
                {
                    return NotFound(new { success = false, message = "No duty data found for the teacher at this time." });
                }

                Console.WriteLine($"Duty found: Teacher: {teacherEmployeeNumber}, Room: {duty.d.RoomId}, Paper: {duty.p.CourseCode}, Time: {duty.p.TimeSlot}");

                var sittingArrangements = await _context.SittingArrangements
                    .Where(sa => sa.RoomId == duty.d.RoomId && sa.PaperId == duty.p.PaperId)
                    .ToListAsync();

                Console.WriteLine($"Found {sittingArrangements.Count} sitting arrangements.");

                var students = await _context.SittingArrangements
                    .Include(sa => sa.Room)
                    .Where(sa => sa.RoomId == duty.d.RoomId && sa.PaperId == duty.p.PaperId)
                    .Join(_context.Students, sa => sa.RegistrationNumber, s => s.RegistrationNumber, (sa, s) => new
                    {
                        s.StudentName,
                        s.RegistrationNumber,
                        sa.Seat,
                        duty.p.CourseCode,
                        duty.p.PaperId,
                        duty.p.Date,
                        duty.p.TimeSlot,
                        RoomNumber = sa.Room.RoomNumber
                    })
                    .ToListAsync();

                if (!students.Any())
                {
                    Console.WriteLine("No students found in the room at the given time.");
                    return NotFound(new { success = false, message = "No students found in the room at the given time." });
                }

                foreach (var student in students)
                {
                    Console.WriteLine($"Student: {student.StudentName}, Registration No: {student.RegistrationNumber}, Seat: {student.Seat}, Room: {student.RoomNumber}, TimeSlot: {student.TimeSlot}");
                }

                return Ok(new { success = true, data = students });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An error occurred: " + ex.Message });
            }
        }


        [HttpPost("submit-attendance-batch")]
        public async Task<IActionResult> SubmitAttendance([FromBody] StudentAttendanceBatchRequest request)
        {
            Console.WriteLine("🔁 Processing attendance batch");

            foreach (var student in request.Students)
            {
                Console.WriteLine($"📌 Checking: {student.RegistrationNumber}, {student.Date}, {student.PaperId}");

                bool alreadyExists = await _context.Attendances.AnyAsync(a =>
                    a.RegistrationNumber == student.RegistrationNumber &&
                    a.TeacherEmployeeNumber == student.TeacherEmployeeNumber &&
                    a.RoomNumber == student.RoomNumber &&
                    a.PaperId == student.PaperId &&
                    a.Date == DateOnly.Parse(student.Date) &&
                    a.TimeSlot == student.TimeSlot &&
                    a.Status == student.AttendanceStatus
                );

                if (alreadyExists)
                {
                    Console.WriteLine($"⏭️ Already exists, skipping: {student.RegistrationNumber}");
                    continue;
                }

                var attendance = new Attendance
                {
                    RegistrationNumber = student.RegistrationNumber,
                    TeacherEmployeeNumber = student.TeacherEmployeeNumber,
                    RoomNumber = student.RoomNumber,
                    PaperId = student.PaperId,
                    Date = DateOnly.Parse(student.Date),
                    TimeSlot = student.TimeSlot,
                    Status = student.AttendanceStatus
                };

                _context.Attendances.Add(attendance);
                Console.WriteLine($"✅ Added: {student.RegistrationNumber}");
            }

            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Attendance submitted (duplicates skipped)" });
        }

    }
    public class StudentAttendanceRequest
{
    public required string RegistrationNumber { get; set; }
    public required string TeacherEmployeeNumber { get; set; }
    public required string RoomNumber { get; set; }
    public required int PaperId { get; set; }
    public required string Date { get; set; } // Keep it string if you're sending it as "2025-04-20"
    public required string TimeSlot { get; set; }
    public DateTime DateAndTime { get; set; }
    public required string AttendanceStatus { get; set; }
}
    public class StudentAttendanceBatchRequest
    {
        public required List<StudentAttendanceRequest> Students { get; set; }
    }
}
