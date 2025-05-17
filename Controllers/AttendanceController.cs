using Exam_Invagilation_System.Models;
using System.Drawing.Printing;
using Microsoft.AspNetCore.Mvc;
using Exam_Invagilation_System.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Exam_Invagilation_System.Controllers
{
    [Authorize(AuthenticationSchemes = "MyCookieAuth")]

    [Route("Attendance")]

    public class AttendanceController : Controller
    {
    private readonly AppDbContext _db;
        public AttendanceController(AppDbContext db)
        {
            _db = db;
        }
        [HttpGet("Index")]

        public IActionResult Index(int pageNumber = 1, int pageSize = 10)
        {
            int totalAttendanceRecords = _db.Attendances.Count();
            int totalPages = (int)Math.Ceiling(totalAttendanceRecords / (double)pageSize);

            var attendance = _db.Attendances
                .Include(a => a.Student)
                .Include(a => a.Teacher)
                .Include(a => a.Room)
                 .Include(a => a.Paper)
                    .ThenInclude(p => p.Course)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var model = new AttendancePaginationViewModel
            {
                Attendances = attendance,
                CurrentPage = pageNumber,
                TotalPages = totalPages,
                PageSize = pageSize
            };

            return View(model);
        }

    }
}
