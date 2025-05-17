using Exam_Invagilation_System.Entities;
using Exam_Invagilation_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Diagnostics;

namespace Exam_Invagilation_System.Controllers
{
    [Authorize(AuthenticationSchemes = "MyCookieAuth")]

    [Route("InsertTeacher")]
    public class InsertTeacherController : Controller
    {
        private readonly AppDbContext _db;

        public InsertTeacherController(AppDbContext db)
        {
            _db = db;
        }

        // ✅ GET: Retrieve paginated list of teachers
        [HttpGet("Teacher")]
        public IActionResult Teacher(int pageNumber = 1, int pageSize = 10)
        {
            int totalTeachers = _db.Teachers.Count();
            int totalPages = (int)Math.Ceiling(totalTeachers / (double)pageSize);

            var teachers = _db.Teachers
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var model = new TeacherPaginationViewModel
            {
                Teachers = teachers,
                CurrentPage = pageNumber,
                TotalPages = totalPages,
                PageSize = pageSize
            };

            return View(model);
        }

        // ✅ POST: Add a new teacher
        [HttpPost("AddTeacher")]
        public IActionResult AddTeacher(Teacher teacher)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Failed to add teacher. Please fill in all required fields.";
                return RedirectToAction("Teacher", new { pageNumber = 1, pageSize = 10 });
            }

            // Check if Teacher Employee Number OR Email already exists
            bool isDuplicate = _db.Teachers.Any(t =>
                t.TeacherEmployeeNumber == teacher.TeacherEmployeeNumber ||
                t.TeacherEmail == teacher.TeacherEmail);

            if (isDuplicate)
            {
                TempData["error"] = "Teacher Employee Number or Email already exists! Please use unique values.";
                return RedirectToAction("Teacher", new { pageNumber = 1, pageSize = 10 });
            }

            var newTeacher = new Teacher
            {
                TeacherEmployeeNumber = teacher.TeacherEmployeeNumber,
                TeacherName = teacher.TeacherName,
                TeacherEmail = teacher.TeacherEmail,
                TeacherDesignation = teacher.TeacherDesignation,
                TeacherDepartment = teacher.TeacherDepartment
            };

            _db.Teachers.Add(newTeacher);
            _db.SaveChanges();

            TempData["success"] = "Teacher added successfully!";
            return RedirectToAction("Teacher", new { pageNumber = 1, pageSize = 10 });
        }

        // ✅ POST: Upload Excel file to add multiple teachers with error handling
        [HttpPost("UploadTeacherExcel")]
        public async Task<IActionResult> UploadTeacherExcel(IFormFile file)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            if (file == null || file.Length <= 0)
            {
                TempData["error"] = "Invalid file. Please upload an Excel file.";
                return RedirectToAction("Teacher", new { pageNumber = 1, pageSize = 10 });
            }

            try
            {
                if (Path.GetExtension(file.FileName).ToLower() != ".xlsx")
                {
                    TempData["error"] = "Invalid file format. Upload a .xlsx file.";
                    return RedirectToAction("Teacher", new { pageNumber = 1, pageSize = 10 });
                }

                using (var package = new ExcelPackage(file.OpenReadStream()))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension?.Rows ?? 0;

                    if (rowCount <= 1)
                    {
                        TempData["error"] = "The Excel file is empty or contains only headers.";
                        return RedirectToAction("Teacher", new { pageNumber = 1, pageSize = 10 });
                    }

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var TeacherEmployeeNumber = worksheet.Cells[row, 1].Text;
                        var TeacherEmail = worksheet.Cells[row, 3].Text;

                        if (string.IsNullOrEmpty(TeacherEmployeeNumber) || string.IsNullOrEmpty(TeacherEmail))
                        {
                            TempData["error"] = $"Row {row}: Employee Number and Email are required.";
                            continue;
                        }

                        var existingTeacher = await _db.Teachers
                            .FirstOrDefaultAsync(t => t.TeacherEmployeeNumber == TeacherEmployeeNumber || t.TeacherEmail == TeacherEmail);

                        var teacher = new Teacher
                        {
                            TeacherEmployeeNumber = TeacherEmployeeNumber,
                            TeacherName = worksheet.Cells[row, 2].Text,
                            TeacherEmail = TeacherEmail,
                            TeacherDesignation = worksheet.Cells[row, 4].Text,
                            TeacherDepartment = worksheet.Cells[row, 5].Text
                        };

                        if (existingTeacher != null)
                        {
                            _db.Entry(existingTeacher).CurrentValues.SetValues(teacher);
                        }
                        else
                        {
                            await _db.Teachers.AddAsync(teacher);
                        }
                    }

                    await _db.SaveChangesAsync();
                    TempData["success"] = "Teachers added from Excel!";
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = $"Error processing file: {ex.Message}";
            }

            return RedirectToAction("Teacher", new { pageNumber = 1, pageSize = 10 });
        }

        // ✅ POST: Edit teacher details
        [HttpPost("EditTeacher")]
        public IActionResult EditTeacher(Teacher teacher)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Invalid data.";
                return RedirectToAction("Teacher", new { pageNumber = 1, pageSize = 10 });
            }

            try
            {
                var teacherToUpdate = _db.Teachers.Find(teacher.TeacherId);
                if (teacherToUpdate == null)
                {
                    TempData["error"] = "Teacher not found!";
                    return RedirectToAction("Teacher", new { pageNumber = 1, pageSize = 10 });
                }

                bool isDuplicate = _db.Teachers
                    .Any(t => (t.TeacherEmployeeNumber == teacher.TeacherEmployeeNumber || t.TeacherEmail == teacher.TeacherEmail)
                        && t.TeacherId != teacher.TeacherId);

                if (isDuplicate)
                {
                    TempData["error"] = "Employee Number or Email already exists!";
                    return RedirectToAction("Teacher", new { pageNumber = 1, pageSize = 10 });
                }

                _db.Entry(teacherToUpdate).CurrentValues.SetValues(teacher);
                _db.SaveChanges();

                TempData["success"] = "Teacher updated successfully!";
            }
            catch (Exception ex)
            {
                TempData["error"] = $"Error updating teacher: {ex.Message}";
            }

            return RedirectToAction("Teacher", new { pageNumber = 1, pageSize = 10 });
        }

        // ✅ POST: Delete teacher
        [HttpPost("DeleteTeacher")]
        public IActionResult DeleteTeacher(string TeacherEmployeeNumber)
        {
            try
            {
                var teacher = _db.Teachers.FirstOrDefault(t => t.TeacherEmployeeNumber == TeacherEmployeeNumber);
                if (teacher == null)
                {
                    TempData["error"] = "Teacher not found!";
                    return RedirectToAction("Teacher", new { pageNumber = 1, pageSize = 10 });
                }

                _db.Teachers.Remove(teacher);
                _db.SaveChanges();

                TempData["success"] = "Teacher deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["error"] = $"Error deleting teacher: {ex.Message}";
            }

            return RedirectToAction("Teacher", new { pageNumber = 1, pageSize = 10 });
        }
    }
}
