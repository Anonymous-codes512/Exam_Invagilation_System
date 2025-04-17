using Exam_Invagilation_System.Entities;
using Exam_Invagilation_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Diagnostics;

namespace Exam_Invagilation_System.Controllers
{
    [Route("InsertRoom")]
    public class InsertRoomController : Controller
    {
        private readonly AppDbContext _db;

        public InsertRoomController(AppDbContext db)
        {
            _db = db;
        }

        // 🏢 Room Controller Functions 🏢
        // ✅ GET: Retrieve paginated list of rooms
        [HttpGet("Room")]
        public IActionResult Room(int pageNumber = 1, int pageSize = 10)
        {
            int totalRooms = _db.Rooms.Count();
            int totalPages = (int)Math.Ceiling(totalRooms / (double)pageSize);

            var rooms = _db.Rooms
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var model = new RoomPaginationViewModel
            {
                Rooms = rooms,
                CurrentPage = pageNumber,
                TotalPages = totalPages,
                PageSize = pageSize
            };

            return View(model);
        }

        // ✅ POST: Add a new room
        [HttpPost("AddRoom")]
        public IActionResult AddRoom(Room room)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Failed to add room. Please fill in all required fields.";
                return RedirectToAction("Room", new { pageNumber = 1, pageSize = 10 });
            }

            // Check if Room Number already exists
            bool isDuplicate = _db.Rooms.Any(r => r.RoomNumber == room.RoomNumber);
            if (isDuplicate)
            {
                TempData["error"] = "Room Number already exists! Please use a unique number.";
                return RedirectToAction("Room", new { pageNumber = 1, pageSize = 10 });
            }

            var newRoom = new Room
            {
                RoomNumber = room.RoomNumber,
                Location = room.Location,
                Columns = room.Columns,
                Rows = room.Rows,
                TotalStrength = room.TotalStrength
            };

            _db.Rooms.Add(newRoom);
            _db.SaveChanges();

            TempData["success"] = "Room added successfully!";
            return RedirectToAction("Room", new { pageNumber = 1, pageSize = 10 });
        }

        // ✅ POST: Upload Excel file to add multiple rooms
        [HttpPost("UploadRoomExcel")]
        public async Task<IActionResult> UploadRoomExcel(IFormFile file)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            if (file == null || file.Length <= 0)
            {
                TempData["error"] = "Invalid file. Please upload an Excel file.";
                return RedirectToAction("Room", new { pageNumber = 1, pageSize = 10 });
            }

            try
            {
                if (Path.GetExtension(file.FileName).ToLower() != ".xlsx")
                {
                    TempData["error"] = "Invalid file format. Upload a .xlsx file.";
                    return RedirectToAction("Room", new { pageNumber = 1, pageSize = 10 });
                }

                using (var package = new ExcelPackage(file.OpenReadStream()))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension?.Rows ?? 0;

                    if (rowCount <= 1)
                    {
                        TempData["error"] = "The Excel file is empty or contains only headers.";
                        return RedirectToAction("Room", new { pageNumber = 1, pageSize = 10 });
                    }

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var RoomNumber = worksheet.Cells[row, 1].Text;
                        if (string.IsNullOrEmpty(RoomNumber)) continue;

                        var existingRoom = await _db.Rooms
                            .FirstOrDefaultAsync(r => r.RoomNumber == RoomNumber);

                        var room = new Room
                        {
                            RoomNumber = RoomNumber,
                            Location = worksheet.Cells[row, 2].Text,
                            Columns = int.TryParse(worksheet.Cells[row, 3].Text, out int columns) ? columns : 0,
                            Rows = int.TryParse(worksheet.Cells[row, 4].Text, out int rows) ? rows : 0,
                            TotalStrength = int.TryParse(worksheet.Cells[row, 5].Text, out int totalStrength) ? totalStrength : 0
                        };

                        if (existingRoom != null)
                        {
                            _db.Entry(existingRoom).CurrentValues.SetValues(room);
                        }
                        else
                        {
                            await _db.Rooms.AddAsync(room);
                        }
                    }

                    await _db.SaveChangesAsync();
                    TempData["success"] = "Rooms added from Excel!";
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = $"Error processing file: {ex.Message}";
            }

            return RedirectToAction("Room", new { pageNumber = 1, pageSize = 10 });
        }

        // ✅ POST: Edit room details
        [HttpPost("EditRoom")]
        public IActionResult EditRoom(Room room)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Invalid data.";
                return RedirectToAction("Room", new { pageNumber = 1, pageSize = 10 });
            }

            try
            {
                var roomToUpdate = _db.Rooms.Find(room.RoomId);
                if (roomToUpdate == null)
                {
                    TempData["error"] = "Room not found!";
                    return RedirectToAction("Room", new { pageNumber = 1, pageSize = 10 });
                }

                bool isDuplicateRoomNo = _db.Rooms
                    .Any(r => r.RoomNumber == room.RoomNumber && r.RoomId != room.RoomId);

                if (isDuplicateRoomNo)
                {
                    TempData["error"] = "Room Number already exists!";
                    return RedirectToAction("Room", new { pageNumber = 1, pageSize = 10 });
                }

                _db.Entry(roomToUpdate).CurrentValues.SetValues(room);
                _db.SaveChanges();

                TempData["success"] = "Room updated successfully!";
            }
            catch (Exception ex)
            {
                TempData["error"] = $"Error updating room: {ex.Message}";
            }

            return RedirectToAction("Room", new { pageNumber = 1, pageSize = 10 });
        }

        // ✅ POST: Delete room
        [HttpPost("DeleteRoom")]
        public IActionResult DeleteRoom(string RoomNumber)
        {
            try
            {
                var room = _db.Rooms.FirstOrDefault(r => r.RoomNumber == RoomNumber);
                if (room == null)
                {
                    TempData["error"] = "Room not found!";
                    return RedirectToAction("Room", new { pageNumber = 1, pageSize = 10 });
                }

                _db.Rooms.Remove(room);
                _db.SaveChanges();

                TempData["success"] = "Room deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["error"] = $"Error deleting room: {ex.Message}";
            }

            return RedirectToAction("Room", new { pageNumber = 1, pageSize = 10 });
        }
    }
}
