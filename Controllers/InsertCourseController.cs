using Exam_Invagilation_System.Entities;
using Exam_Invagilation_System.Models;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Diagnostics;

namespace Exam_Invagilation_System.Controllers
{
    [Route("InsertCourse")]
    public class InsertCourseController : Controller
    {
        private readonly AppDbContext _db;

        public InsertCourseController(AppDbContext db)
        {
            _db = db;
        }
        // 📚 Course Controller Functions 📚
        // ✅ GET: Retrieve paginated list of courses
        [HttpGet("Course")]
        public IActionResult Course(int pageNumber = 1, int pageSize = 10)
        {
            var courses = _db.Courses.Include(c => c.Teacher)
                           .OrderBy(c => c.CourseCode)
                           .Skip((pageNumber - 1) * pageSize)
                           .Take(pageSize)
                           .ToList();

            var teachers = _db.Teachers.ToList(); // Fetch teachers list

            var model = new CoursePaginationViewModel
            {
                Courses = courses,
                Teachers = teachers,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling((double)_db.Courses.Count() / pageSize),
                PageSize = pageSize,
                TeacherEmployeeNumber = "" // Initialize
            };

            return View(model);
        }



        // ✅ POST: Add a new course
        [HttpPost("AddCourse")]
        public IActionResult AddCourse(Course course)
        {
            Console.WriteLine($"TeacherEmployeeNumber: {course.TeacherEmployeeNumber}");
            ModelState.Remove("Teacher");
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                TempData["error"] = "Failed to add course.Please fill all fields";
                Console.WriteLine("Failed to add course.Errors: " + string.Join(", ", errors));
                return RedirectToAction("Course", new { pageNumber = 1, pageSize = 10 });
            }


            // Check if Course Code already exists
            bool isDuplicate = _db.Courses.Any(c => c.CourseCode == course.CourseCode);
            if (isDuplicate)
            {
                TempData["error"] = "Course code already exists! Please use a unique code.";
                return RedirectToAction("Course", new { pageNumber = 1, pageSize = 10 });
            }

            var newCourse = new Course
            {
                CourseCode = course.CourseCode,
                CourseName = course.CourseName,
                PreRequisite = course.PreRequisite,
                TeacherEmployeeNumber = course.TeacherEmployeeNumber,
            };

            _db.Courses.Add(newCourse);
            _db.SaveChanges();

            TempData["success"] = "Course added successfully!";
            return RedirectToAction("Course", new { pageNumber = 1, pageSize = 10 });
        }

        //✅ POST: Upload Excel file to add multiple courses
        [HttpPost("UploadCourseExcel")]
        public async Task<IActionResult> UploadCourseExcel(IFormFile file)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            if (file == null || file.Length <= 0)
            {
                TempData["error"] = "Invalid file. Please upload an Excel file.";
                return RedirectToAction("Course", new { pageNumber = 1, pageSize = 10 });
            }

            try
            {
                if (Path.GetExtension(file.FileName).ToLower() != ".xlsx")
                {
                    TempData["error"] = "Invalid file format. Upload a .xlsx file.";
                    return RedirectToAction("Course", new { pageNumber = 1, pageSize = 10 });
                }

                using (var package = new ExcelPackage(file.OpenReadStream()))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension?.Rows ?? 0;

                    if (rowCount <= 1)
                    {
                        TempData["error"] = "The Excel file is empty or contains only headers.";
                        return RedirectToAction("Course", new { pageNumber = 1, pageSize = 10 });
                    }

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var CourseCode = worksheet.Cells[row, 1].Text.Trim();
                        var CourseName = worksheet.Cells[row, 2].Text.Trim();
                        var PreRequisite = worksheet.Cells[row, 3].Text.Trim();
                        var TeacherEmployeeNumber = worksheet.Cells[row, 4].Text.Trim(); // Added Teacher Column

                        if (string.IsNullOrEmpty(CourseCode) || string.IsNullOrEmpty(TeacherEmployeeNumber))
                        {
                            continue; // Skip invalid rows
                        }

                        var existingCourse = await _db.Courses.FirstOrDefaultAsync(c => c.CourseCode == CourseCode);

                        if (existingCourse != null)
                        {
                            existingCourse.CourseName = CourseName;
                            existingCourse.PreRequisite = PreRequisite;
                            existingCourse.TeacherEmployeeNumber = TeacherEmployeeNumber; // Updating teacher if changed
                        }
                        else
                        {
                            var newCourse = new Course
                            {
                                CourseCode = CourseCode,
                                CourseName = CourseName,
                                PreRequisite = PreRequisite,
                                TeacherEmployeeNumber = TeacherEmployeeNumber, // Assign teacher
                            };

                            await _db.Courses.AddAsync(newCourse);
                        }
                    }

                    await _db.SaveChangesAsync();
                    TempData["success"] = "Courses added from Excel!";
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = $"Error processing file: {ex.Message}";
                Console.WriteLine($"Error processing file: {ex.Message}");
            }

            return RedirectToAction("Course", new { pageNumber = 1, pageSize = 10 });
        }



        // ✅ POST: Edit course details
        [HttpPost("EditCourse")]
        public IActionResult EditCourse(Course course)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Invalid data.";
                return RedirectToAction("Course", new { pageNumber = 1, pageSize = 10 });
            }

            try
            {
                var courseToUpdate = _db.Courses.Find(course.CourseId);
                if (courseToUpdate == null)
                {
                    TempData["error"] = "Course not found!";
                    return RedirectToAction("Course", new { pageNumber = 1, pageSize = 10 });
                }

                bool isDuplicateCourseCode = _db.Courses
                    .Any(c => c.CourseCode == course.CourseCode && c.CourseId != course.CourseId);

                if (isDuplicateCourseCode)
                {
                    TempData["error"] = "Course Code already exists!";
                    return RedirectToAction("Course", new { pageNumber = 1, pageSize = 10 });
                }

                // Manually update properties (excluding primary key)
                //courseToUpdate.CourseCode = course.CourseCode;
                courseToUpdate.CourseName = course.CourseName;
                courseToUpdate.PreRequisite = course.PreRequisite;

                _db.SaveChanges();

                TempData["success"] = "Course updated successfully!";
            }
            catch (Exception ex)
            {
                TempData["error"] = $"Error updating course: {ex.Message}";
            }

            return RedirectToAction("Course", new { pageNumber = 1, pageSize = 10 });
        }


        // ✅ POST: Delete course
        [HttpPost("DeleteCourse")]
        public IActionResult DeleteCourse(string CourseCode)
        {
            try
            {
                var course = _db.Courses.FirstOrDefault(c => c.CourseCode== CourseCode);
                if (course== null)
                {
                    TempData["error"] = "Course not found!";
                    return RedirectToAction("Course", new { pageNumber = 1, pageSize = 10 });
                }

                _db.Courses.Remove(course);
                _db.SaveChanges();

                TempData["success"] = "Course deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["error"] = $"Error deleting course: {ex.Message}";
                Console.WriteLine($"Error deleting course: {ex.Message}");
            }

            return RedirectToAction("Course", new { pageNumber = 1, pageSize = 10 });
        }
    }
}
