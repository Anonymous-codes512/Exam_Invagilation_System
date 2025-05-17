using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Exam_Invagilation_System.Models;
using Exam_Invagilation_System.Entities;
using OfficeOpenXml;
using Microsoft.AspNetCore.Authorization;

namespace Exam_Invagilation_System.Controllers
{
    [Authorize(AuthenticationSchemes = "MyCookieAuth")]

    public class DateSheetController : Controller
    {
        private readonly AppDbContext _db;

        public DateSheetController(AppDbContext db)
        {
            _db = db;
        }

        // 📅 Fetch DateSheet Data
        public IActionResult DateSheet(int pageNumber = 1, int pageSize = 10)
        {
            // Fetch DateSheet data, grouped by Date (same date will appear in a single row)
            var dateSheets = _db.Papers
                .Include(ds => ds.Room)
                .Include(ds => ds.Course)
                .OrderBy(ds => ds.Date)
                .GroupBy(ds => ds.Date) // Group by Date
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(group => new DateSheetViewModel
                {
                    Date = group.Key, // The grouped date
                    Papers = group.Select(ds => new PaperViewModel
                    {
                        PaperId = ds.PaperId, // Include PaperId
                        CourseCode = ds.Course.CourseCode,
                        CourseName = ds.Course.CourseName,
                        RoomNumber = ds.Room.RoomNumber,
                        Location = ds.Room.Location,
                        TimeSlot = ds.TimeSlot,
                        ExamTerm = ds.ExamTerm
                    }).ToList()
                })
                .ToList();

            // Get all courses and rooms for filtering (optional)
            var Courses = _db.Courses.ToList();
            var Rooms = _db.Rooms.ToList();

            // Calculate the total number of distinct dates for pagination
            var totalPages = (int)Math.Ceiling((double)_db.Papers
                .Select(ds => ds.Date)
                .Distinct()
                .Count() / pageSize);

            // Prepare the model for the view
            var model = new DateSheetPaginationViewModel
            {
                DateSheets = dateSheets, // Pass the grouped date sheets
                Courses = Courses,
                Rooms = Rooms,
                CurrentPage = pageNumber,
                TotalPages = totalPages,
                PageSize = pageSize,
            };

            return View(model);
        }


        [HttpPost("AddDateSheet")]
        public IActionResult AddDateSheet(Paper paper, string StartTime, string EndTime)
        {
            // Log the received Paper data for debugging
            Console.WriteLine($"Received Paper data: CourseCode = {paper.CourseCode}, RoomId = {paper.RoomId}");

            // Log all ModelState errors
            Console.WriteLine("ModelState Errors:");
            foreach (var modelError in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine($"Error: {modelError.ErrorMessage}");
            }

            // Manually check for required fields before ModelState validation
            if (string.IsNullOrEmpty(paper.CourseCode))
            {
                ModelState.AddModelError("CourseCode", "Course is required.");
            }

            if (paper.RoomId <= 0)
            {
                ModelState.AddModelError("RoomId", "Room is required.");
            }

            // Check if the course exists
            var courseExists = _db.Courses.Any(c => c.CourseCode == paper.CourseCode);
            if (!courseExists)
            {
                ModelState.AddModelError("CourseCode", $"Course with code {paper.CourseCode} not found.");
                return RedirectToAction("DateSheet", new { pageNumber = 1, pageSize = 10 });
            }

            // Check if the room exists
            var roomExists = _db.Rooms.Any(r => r.RoomId == paper.RoomId);
            if (!roomExists)
            {
                ModelState.AddModelError("RoomId", $"Room with ID {paper.RoomId} not found.");
                return RedirectToAction("DateSheet", new { pageNumber = 1, pageSize = 10 });
            }

            // Generate TimeSlot by combining StartTime and EndTime
            string timeSlot = $"{StartTime}-{EndTime}";

            // Create a new Paper object and populate it
            var newDateSheet = new Paper
            {
                CourseCode = paper.CourseCode,
                RoomId = paper.RoomId,
                Date = paper.Date,
                TimeSlot = timeSlot,
                ExamTerm = paper.ExamTerm
            };

            // Add the new Paper record to the database
            _db.Papers.Add(newDateSheet);
            _db.SaveChanges();

            // Success message
            TempData["success"] = "DateSheet added successfully!";
            return RedirectToAction("DateSheet", new { pageNumber = 1, pageSize = 10 });
        }



        [HttpPost("UploadDateSheetExcel")]
        public async Task<IActionResult> UploadDateSheetExcel(IFormFile file)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Check if the file is valid
            if (file == null || file.Length <= 0)
            {
                TempData["error"] = "Invalid file. Please upload an Excel file.";
                return RedirectToAction("DateSheet", new { pageNumber = 1, pageSize = 10 });
            }

            try
            {
                // Validate the file format
                if (Path.GetExtension(file.FileName).ToLower() != ".xlsx")
                {
                    TempData["error"] = "Invalid file format. Upload a .xlsx file.";
                    return RedirectToAction("DateSheet", new { pageNumber = 1, pageSize = 10 });
                }

                using (var package = new ExcelPackage(file.OpenReadStream()))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension?.Rows ?? 0;

                    // Ensure the file is not empty
                    if (rowCount <= 1)
                    {
                        TempData["error"] = "The Excel file is empty or contains only headers.";
                        return RedirectToAction("DateSheet", new { pageNumber = 1, pageSize = 10 });
                    }

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var courseCode = worksheet.Cells[row, 1].Text.Trim();
                        var roomId = worksheet.Cells[row, 2].Text.Trim();
                        var date = worksheet.Cells[row, 3].Text.Trim();
                        var timeSlot = worksheet.Cells[row, 4].Text.Trim();
                        var examTerm = worksheet.Cells[row, 5].Text.Trim();

                        // Skip invalid rows
                        if (string.IsNullOrEmpty(courseCode) || string.IsNullOrEmpty(roomId) || string.IsNullOrEmpty(date) || string.IsNullOrEmpty(timeSlot) || string.IsNullOrEmpty(examTerm))
                        {
                            continue; // Skip rows with missing data
                        }

                        if (!int.TryParse(roomId, out int roomIdInt))
                        {
                            TempData["error"] = $"Invalid RoomId format for course {courseCode}.";
                            return RedirectToAction("DateSheet", new { pageNumber = 1, pageSize = 10 });
                        }

                        // Check if the course exists
                        var course = await _db.Courses.FirstOrDefaultAsync(c => c.CourseCode == courseCode);
                        var room = await _db.Rooms.FirstOrDefaultAsync(r => r.RoomId == roomIdInt);

                        if (course == null)
                        {
                            TempData["error"] = $"Course with code {courseCode} not found.";
                            return RedirectToAction("DateSheet", new { pageNumber = 1, pageSize = 10 });
                        }

                        if (room == null)
                        {
                            TempData["error"] = $"Room with ID {roomId} not found.";
                            return RedirectToAction("DateSheet", new { pageNumber = 1, pageSize = 10 });
                        }

                        // Convert date to DateTime
                        DateOnly examDate;
                        if (!DateOnly.TryParse(date, out examDate))
                        {
                            TempData["error"] = $"Invalid date format for {courseCode} in room {roomId}.";
                            return RedirectToAction("DateSheet", new { pageNumber = 1, pageSize = 10 });
                        }

                        // Create and save the Paper entity
                        var paper = new Paper
                        {
                            CourseCode = courseCode,
                            RoomId = room.RoomId,  // Use the RoomId from the Room entity
                            Date = examDate,
                            TimeSlot = timeSlot,
                            ExamTerm = examTerm
                        };

                        // Save the new Paper entity to the database
                        await _db.Papers.AddAsync(paper);
                    }

                    // Save all the data to the database
                    await _db.SaveChangesAsync();
                    TempData["success"] = "DateSheet data uploaded and saved successfully!";
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                TempData["error"] = $"Error processing file: {ex.Message}";
                Console.WriteLine($"Error processing file: {ex.Message}");
            }

            return RedirectToAction("DateSheet", new { pageNumber = 1, pageSize = 10 });
        }


        [HttpPost("EditDateSheet")]
        public async Task<IActionResult> EditDateSheet(Paper paper)
        {
            try
            {
                Console.WriteLine(paper.PaperId);
                var paperToUpdate = await _db.Papers.FindAsync(paper.PaperId);

                if (paperToUpdate == null)
                {
                    TempData["error"] = "DateSheet not found!";
                    return RedirectToAction("DateSheet", new { pageNumber = 1, pageSize = 10 });
                }

                // Check for duplicate CourseCode in the same room and exam term (optional check)
                bool isDuplicatePaper = _db.Papers
                    .Any(p => p.CourseCode == paper.CourseCode && p.RoomId == paper.RoomId && p.ExamTerm == paper.ExamTerm && p.PaperId != paper.PaperId);

                if (isDuplicatePaper)
                {
                    TempData["error"] = "Duplicate entry for this course in the same room and term!";
                    return RedirectToAction("DateSheet", new { pageNumber = 1, pageSize = 10 });

                }

                paperToUpdate.CourseCode = paper.CourseCode;
                paperToUpdate.RoomId = paper.RoomId;
                paperToUpdate.Date = paper.Date;
                paperToUpdate.TimeSlot = paper.TimeSlot;
                paperToUpdate.ExamTerm = paper.ExamTerm;

                // Save the updated record to the database
                await _db.SaveChangesAsync();

                TempData["success"] = "DateSheet updated successfully!";
            }
            catch (Exception ex)
            {
                TempData["error"] = $"Error updating DateSheet: {ex.Message}";
            }
            return RedirectToAction("DateSheet", new { pageNumber = 1, pageSize = 10 });
        }

        [HttpPost("DeleteDateSheet")]
        public async Task<IActionResult> DeleteDateSheet(int paperId)
        {
            try
            {
                // Find the Paper record by PaperId
                var paper = await _db.Papers.FindAsync(paperId);
                if (paper == null)
                {
                    TempData["error"] = "DateSheet not found!";
                    return RedirectToAction("DateSheet", new { pageNumber = 1, pageSize = 10 });
                }

                // Remove the paper record from the database
                _db.Papers.Remove(paper);
                await _db.SaveChangesAsync();

                TempData["success"] = "DateSheet deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["error"] = $"Error deleting DateSheet: {ex.Message}";
                Console.WriteLine($"Error deleting DateSheet: {ex.Message}");
            }
            return RedirectToAction("DateSheet", new { pageNumber = 1, pageSize = 10 });
        }

    }
}
