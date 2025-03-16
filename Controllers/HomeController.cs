using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Exam_Invagilation_System.Models;
using Exam_Invagilation_System.Entities;
using System.Linq;

namespace Exam_Invagilation_System.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            // Fetch student attendance
            var students = _context.Students.ToList();

            int studentsAppeared = students.Count(s => s.Attendance > 80);

            int totalStudents = students.Count;
            int notAppeared = totalStudents - studentsAppeared;

            //Fetch Cheating Reports with Student Name &Teacher Name
           var cheatingReports = _context.CheatingReports
               .Include(cr => cr.Student)
               .Include(cr => cr.Teacher)
               .Select(cr => new
               {
                   cr.CheatingReportId,
                   StudentName = cr.Student != null ? cr.Student.StudentName : "Unknown",
                   TeacherName = cr.Teacher != null ? cr.Teacher.TeacherName : "Unknown",
                   cr.RoomId,
                   cr.CourseCode,
                   cr.Date,
                   cr.TimeSlot,
                   cr.Description
               })
               .ToList();

            ViewData["TotalStudents"] = totalStudents;
            ViewData["StudentsAppeared"] = studentsAppeared;
            ViewData["NotAppeared"] = notAppeared;
            ViewData["CheatingReports"] = cheatingReports;

            return View();
        }
    }
}
