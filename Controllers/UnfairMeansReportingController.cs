using Exam_Invagilation_System.Entities;
using Exam_Invagilation_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Exam_Invagilation_System.Controllers
{
    [Authorize(AuthenticationSchemes = "MyCookieAuth")]

    [Route("UnfairMeansReporting")]
    public class UnfairMeansReportingController : Controller
    {
        private readonly AppDbContext _db;
        public UnfairMeansReportingController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet("Index")]
        public IActionResult Index(int pageNumber = 1, int pageSize = 10)
        {
            int totalCases = _db.CheatingReports.Count();
            int totalPages = (int)Math.Ceiling(totalCases / (double)pageSize);

            var UnfairCases = _db.CheatingReports
                .Include(uc => uc.Student)
                .Include(uc => uc.Teacher)
                .Include(uc => uc.Room)
                .Include(uc => uc.Course)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var model = new UnfairMeansReportingPaginationViewModel
            {
                UnfairCases = UnfairCases,
                CurrentPage = pageNumber,
                TotalPages = totalPages,
                PageSize = pageSize
            };

            return View(model);
        }
    }
}
