using Exam_Invagilation_System.Entities;
using Exam_Invagilation_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Invagilation_System.Controllers
{
    [Route("CourseRegister")]
    public class CourseRegisterController : Controller
    {
        private readonly AppDbContext _db;

        public CourseRegisterController(AppDbContext db)
        {
            _db = db;
        }

        // 📚 Course Registration Controller Functions 📚

        // ✅ GET: Retrieve paginated list of student-course registrations
        
        [HttpGet("StudentCourses")]
        public IActionResult StudentCourses(int pageNumber = 1, int pageSize = 10)
        {
            // Get distinct RegistrationNumbers first, then include related data (Student, Course, and Teacher)
            var studentCourses = _db.StudentCourses
                .Include(sc => sc.Student)  // Include Student data
                .Include(sc => sc.Course)   // Include Course data
                .ThenInclude(course => course.Teacher)  // Include Teacher data within each Course
                .OrderBy(sc => sc.RegistrationNumber)
                .GroupBy(sc => sc.RegistrationNumber)  // Group by RegistrationNumber
                .Skip((pageNumber - 1) * pageSize)    // Pagination (skip previous pages)
                .Take(pageSize)                       // Take only 'pageSize' number of records
                .Select(group => new StudentCourseViewModel
                {
                    RegistrationNumber = group.Key,  // Registration Number for the group
                    StudentName = group.First().Student.StudentName,  // Get the StudentName from the first record in the group
                    Courses = group.Select(sc => new CourseViewModel
                    {
                        CourseCode = sc.CourseCode,
                        CourseName = sc.Course.CourseName,
                        TeacherName = sc.Course.Teacher.TeacherName,  // Assuming Teacher is included in the Course entity
                        TeacherEmployeeNumber = sc.Course.Teacher.TeacherEmployeeNumber  // Assuming TeacherEmployeeNumber is part of Course
                    }).ToList()  // List of courses for the student
                }).ToList();

            // Get all students and courses to be used in filtering (optional)
            var students = _db.Students.ToList();
            var courses = _db.Courses.ToList();

            // Calculate total pages based on distinct RegistrationNumbers
            var totalPages = (int)Math.Ceiling((double)_db.StudentCourses
                .Select(sc => sc.RegistrationNumber)  // Select distinct RegistrationNumbers
                .Distinct()                          // Ensure distinct RegistrationNumbers
                .Count() / pageSize);

            var model = new StudentCoursePaginationViewModel
            {
                StudentCourses = studentCourses,
                Students = students,
                Courses = courses,
                CurrentPage = pageNumber,
                TotalPages = totalPages,
                PageSize = pageSize,
                RegistrationNumber = "",
                CourseCode = ""
            };

            return View(model);
        }

        // ✅ POST: Upload Excel file to register multiple students in courses
        [HttpPost("UploadStudentCoursesExcel")]
        public async Task<IActionResult> UploadStudentCoursesExcel(IFormFile file) {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            if (file == null || file.Length <= 0)
            {
                TempData["error"] = "Invalid file. Please upload an Excel file.";
                return RedirectToAction("StudentCourses", new { pageNumber = 1, pageSize = 10 });
            }

            try
            {
                if (Path.GetExtension(file.FileName).ToLower() != ".xlsx")
                {
                    TempData["error"] = "Invalid file format. Upload a .xlsx file.";
                    return RedirectToAction("StudentCourses", new { pageNumber = 1, pageSize = 10 });
                }

                using (var package = new ExcelPackage(file.OpenReadStream()))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension?.Rows ?? 0;

                    if (rowCount <= 1)
                    {
                        TempData["error"] = "The Excel file is empty or contains only headers.";
                        return RedirectToAction("StudentCourses", new { pageNumber = 1, pageSize = 10 });
                    }

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var RegistrationNumber = worksheet.Cells[row, 1].Text.Trim();
                        var CourseCode = worksheet.Cells[row, 2].Text.Trim();

                        if (string.IsNullOrEmpty(RegistrationNumber) || string.IsNullOrEmpty(CourseCode))
                        {
                            continue; // Skip invalid rows
                        }

                        var student = await _db.Students.FirstOrDefaultAsync(s => s.RegistrationNumber == RegistrationNumber);
                        var course = await _db.Courses.FirstOrDefaultAsync(c => c.CourseCode == CourseCode);

                        if (student == null || course == null)
                        {
                            continue; // Skip rows with invalid student or course
                        }

                        var existingRegistration = await _db.StudentCourses
                            .FirstOrDefaultAsync(sc => sc.RegistrationNumber == RegistrationNumber && sc.CourseCode == CourseCode);

                        if (existingRegistration == null)
                        {
                            var newStudentCourse = new StudentCourse
                            {
                                RegistrationNumber = RegistrationNumber,
                                CourseCode = CourseCode
                            };

                            await _db.StudentCourses.AddAsync(newStudentCourse);
                        }
                    }

                    await _db.SaveChangesAsync();
                    TempData["success"] = "Students registered from Excel!";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing file: {ex.Message}");
                TempData["error"] = $"Error processing file: {ex.Message}";
            }

            return RedirectToAction("StudentCourses", new { pageNumber = 1, pageSize = 10 });
        }

        // ✅ POST: Edit student-course registration
        //[HttpPost("EditStudentCourse")]
        //public IActionResult EditStudentCourse(StudentCourse studentCourse)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        TempData["error"] = "Invalid data.";
        //        return RedirectToAction("StudentCourses", new { pageNumber = 1, pageSize = 10 });
        //    }

        //    try
        //    {
        //        var studentCourseToUpdate = _db.StudentCourses
        //            .FirstOrDefault(sc => sc.RegistrationNumber == studentCourse.RegistrationNumber && sc.CourseCode == studentCourse.CourseCode);

        //        if (studentCourseToUpdate == null)
        //        {
        //            TempData["error"] = "Student not registered for this course.";
        //            return RedirectToAction("StudentCourses", new { pageNumber = 1, pageSize = 10 });
        //        }

        //        // Update the registration (in case anything needs to change)
        //        _db.SaveChanges();

        //        TempData["success"] = "Student-course registration updated successfully!";
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["error"] = $"Error updating registration: {ex.Message}";
        //    }

        //    return RedirectToAction("StudentCourses", new { pageNumber = 1, pageSize = 10 });
        //}

        // ✅ POST: Delete student-course registration
        [HttpPost("DeleteStudentCourse")]
        public IActionResult DeleteStudentCourse(string RegistrationNumber, string CourseCode)
        {
            try
            {
                var studentCourse = _db.StudentCourses
                    .FirstOrDefault(sc => sc.RegistrationNumber == RegistrationNumber && sc.CourseCode == CourseCode);

                if (studentCourse == null)
                {
                    TempData["error"] = "Student not registered for this course.";
                    return RedirectToAction("StudentCourses", new { pageNumber = 1, pageSize = 10 });
                }

                _db.StudentCourses.Remove(studentCourse);
                _db.SaveChanges();

                TempData["success"] = "Student-course registration deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["error"] = $"Error deleting registration: {ex.Message}";
            }

            return RedirectToAction("StudentCourses", new { pageNumber = 1, pageSize = 10 });
        }
    }
}
