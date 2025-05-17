using Exam_Invagilation_System.Entities;
using Exam_Invagilation_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Exam_Invagilation_System.Controllers
{
    [Authorize(AuthenticationSchemes = "MyCookieAuth")]

    [Route("PaperSummary")]

    public class PaperSummaryController : Controller
    {
        private readonly AppDbContext _db;
        public PaperSummaryController(AppDbContext db)
        {
            _db = db;
        }
        [HttpGet("Index")]
        public IActionResult Index(int pageNumber = 1, int pageSize = 10)
        {
            int totalPaperSummary = _db.PaperSummaryCollections.Count();
            int totalPages = (int)Math.Ceiling(totalPaperSummary / (double)pageSize);

            var paperSummaryCollection = _db.PaperSummaryCollections
                .Include(ps => ps.Teacher)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var model = new PaperSummaryCollectionPaginationViewModel
            {
                PaperSummaryCollection = paperSummaryCollection,
                CurrentPage = pageNumber,
                TotalPages = totalPages,
                PageSize = pageSize
            };

            return View(model);
        }

    }
}
