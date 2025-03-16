using Exam_Invagilation_System.Entities;
using Exam_Invagilation_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Diagnostics;

namespace Exam_Invagilation_System.Controllers
{
    [Route("InsertStudent")]
    public class InsertStudentController : Controller
    {
        private readonly AppDbContext _db;

        public InsertStudentController(AppDbContext db)
        {
            _db = db;
        }
        //🧑‍🎓 Student Controller Functions🧑‍🎓
        // ✅ Fix: Explicitly define as a GET method
        [HttpGet("Student")]
        public IActionResult Student(int pageNumber = 1, int pageSize = 10)
        {
            int totalStudents = _db.Students.Count();
            int totalPages = (int)Math.Ceiling(totalStudents / (double)pageSize);

            var students = _db.Students
                .Include(s => s.StudentCourses)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var model = new StudentPaginationViewModel
            {
                Students = students,
                CurrentPage = pageNumber,
                TotalPages = totalPages,
                PageSize = pageSize
            };

            return View(model);
        }

        // ✅ Fix: Changed route to "AddStudent" to avoid conflicts
        [HttpPost("AddStudent")]
        public IActionResult AddStudent(Student student)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Failed to add student. Please fill in all required fields.";
                return RedirectToAction("Student", new { pageNumber = 1, pageSize = 10 });
            }

            // Check if Registration Number already exists
            bool isDuplicate = _db.Students.Any(s => s.RegistrationNumber == student.RegistrationNumber);
            if (isDuplicate)
            {
                TempData["error"] = "Registration Number already exists! Please use a unique number.";
                return RedirectToAction("Student", new { pageNumber = 1, pageSize = 10 });
            }

            // Validate Attendance range
            if (student.Attendance < 0 || student.Attendance > 100)
            {
                TempData["error"] = "Attendance must be between 0 and 100.";
                return RedirectToAction("Student", new { pageNumber = 1, pageSize = 10 });
            }

            // Ensure no field is empty
            if (string.IsNullOrWhiteSpace(student.RegistrationNumber) ||
                string.IsNullOrWhiteSpace(student.StudentName) ||
                string.IsNullOrWhiteSpace(student.FatherName) ||
                string.IsNullOrWhiteSpace(student.Program) ||
                string.IsNullOrWhiteSpace(student.Section) ||
                student.Semester <= 0) // Assuming semester must be a positive number
            {
                TempData["error"] = "All fields are required. Please fill in all the details.";
                return RedirectToAction("Student", new { pageNumber = 1, pageSize = 10 });
            }

            var newStudent = new Student
            {
                RegistrationNumber = student.RegistrationNumber,
                StudentName = student.StudentName,
                FatherName = student.FatherName,
                Program = student.Program,
                Semester = student.Semester,
                Section = student.Section,
                Attendance = student.Attendance,
                DuesClear = student.DuesClear
            };

            _db.Students.Add(newStudent);
            _db.SaveChanges();

            TempData["success"] = "Student added successfully!";
            return RedirectToAction("Student", new { pageNumber = 1, pageSize = 10 });
        }


        // ✅ Fix: Renamed route for UploadExcel
        [HttpPost("UploadExcel")]
        public async Task<IActionResult> UploadExcel(IFormFile file)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            if (file == null || file.Length <= 0)
            {
                TempData["error"] = "Invalid file. Please upload an Excel file.";
                return RedirectToAction("Student", new { pageNumber = 1, pageSize = 10 });
            }

            try
            {
                if (Path.GetExtension(file.FileName).ToLower() != ".xlsx")
                {
                    TempData["error"] = "Invalid file format. Upload a .xlsx file.";
                    return RedirectToAction("Student", new { pageNumber = 1, pageSize = 10 });
                }

                using (var package = new ExcelPackage(file.OpenReadStream()))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension?.Rows ?? 0;

                    if (rowCount <= 1)
                    {
                        TempData["error"] = "The Excel file is empty or contains only headers.";
                        return RedirectToAction("Student", new { pageNumber = 1, pageSize = 10 });
                    }

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var registrationNumber = worksheet.Cells[row, 1].Text;
                        if (string.IsNullOrEmpty(registrationNumber)) continue;

                        var existingStudent = await _db.Students
                            .FirstOrDefaultAsync(s => s.RegistrationNumber == registrationNumber);

                        var student = new Student
                        {
                            RegistrationNumber = registrationNumber,
                            StudentName = worksheet.Cells[row, 2].Text,
                            FatherName = worksheet.Cells[row, 3].Text,
                            Program = worksheet.Cells[row, 4].Text,
                            Semester = int.TryParse(worksheet.Cells[row, 5].Text, out int semester) ? semester : 1,
                            Section = worksheet.Cells[row, 6].Text,
                            Attendance = int.TryParse(worksheet.Cells[row, 7].Text, out int attendance) ? attendance : 0,
                            DuesClear = worksheet.Cells[row, 8].Text.ToLower() == "yes"
                        };

                        if (existingStudent != null)
                        {
                            _db.Entry(existingStudent).CurrentValues.SetValues(student);
                        }
                        else
                        {
                            await _db.Students.AddAsync(student);
                        }
                    }

                    await _db.SaveChangesAsync();
                    TempData["success"] = "Students added from Excel!";
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = $"Error processing file: {ex.Message}";
            }

            return RedirectToAction("Student", new { pageNumber = 1, pageSize = 10 });
        }

        // ✅ Fix: Ensure unique route for EditStudent
        [HttpPost("EditStudent")]
        public IActionResult EditStudent(Student student)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Invalid data.";
                return RedirectToAction("Student", new { pageNumber = 1, pageSize = 10 });
            }

            try
            {
                var studentToUpdate = _db.Students.Find(student.StudentId);
                if (studentToUpdate == null)
                {
                    TempData["error"] = "Student not found!";
                    return RedirectToAction("Student", new { pageNumber = 1, pageSize = 10 });
                }

                bool isDuplicateRegNumber = _db.Students
                    .Any(s => s.RegistrationNumber == student.RegistrationNumber && s.StudentId != student.StudentId);

                if (isDuplicateRegNumber)
                {
                    TempData["error"] = "Registration Number already exists!";
                    return RedirectToAction("Student", new { pageNumber = 1, pageSize = 10 });
                }

                _db.Entry(studentToUpdate).CurrentValues.SetValues(student);
                _db.SaveChanges();

                TempData["success"] = "Student updated successfully!";
            }
            catch (Exception ex)
            {
                TempData["error"] = $"Error updating student: {ex.Message}";
            }

            return RedirectToAction("Student", new { pageNumber = 1, pageSize = 10 });
        }

        // ✅ Fix: Ensure unique route for DeleteStudent
        [HttpPost("DeleteStudent")]
        public IActionResult DeleteStudent(string registrationNumber)
        {
            try
            {
                var student = _db.Students.FirstOrDefault(s => s.RegistrationNumber == registrationNumber);
                if (student == null)
                {
                    TempData["error"] = "Student not found!";
                    return RedirectToAction("Student", new { pageNumber = 1, pageSize = 10 });
                }

                _db.Students.Remove(student);
                _db.SaveChanges();

                TempData["success"] = "Student deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["error"] = $"Error deleting student: {ex.Message}";
            }

            return RedirectToAction("Student", new { pageNumber = 1, pageSize = 10 });
        }
    }
}
     