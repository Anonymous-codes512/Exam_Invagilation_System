using Exam_Invagilation_System.Entities;
using Exam_Invagilation_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Invagilation_System.Controllers
{
    [Route("TeacherDuty")]
    public class TeacherDutyController : Controller
    {
        private readonly AppDbContext _db;

        public TeacherDutyController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index(int pageNumber = 1, int pageSize = 10)
        {
            // Eager load Duty with related entities (Teacher, Room, Paper, and Course)
            var teacherDuties = _db.Duties
                .Include(d => d.Teacher)  // Include Teacher information
                .Include(d => d.Room)     // Include Room information
                .Include(d => d.Paper)    // Include Paper to access Course
                .ThenInclude(p => p.Course) // Include Course information related to Paper
                .OrderBy(d => d.Date)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Get total count of Duties for pagination
            var totalDuties = _db.Duties.Count();

            // Calculate the total number of pages
            var totalPages = (int)Math.Ceiling((double)totalDuties / pageSize);

            // Create the ViewModel with the paginated data
            var model = new TeacherDutyPaginationViewModel
            {
                TeacherDuties = teacherDuties,
                CurrentPage = pageNumber,
                TotalPages = totalPages,
                PageSize = pageSize,
            };

            return View(model);
        }

        [HttpPost("GenerateTeacherDuties")]
        public async Task<IActionResult> GenerateTeacherDuties()
        {
            // Get all papers from the database
            var papers = await _db.Papers
                .Include(p => p.Room)
                .Include(p => p.Course)
                .ToListAsync();

            // Get all teachers from the database
            var teachers = await _db.Teachers.ToListAsync();

            // Ensure there are teachers available
            if (teachers.Count == 0)
            {
                ModelState.AddModelError("", "There are no teachers available.");
                return RedirectToAction("Index");
            }

            // Delete existing duties for the current papers
            var existingDuties = await _db.Duties
                .Where(d => papers.Select(p => p.PaperId).Contains(d.PaperId))
                .ToListAsync();

            _db.Duties.RemoveRange(existingDuties);  // Remove previous duties
            await _db.SaveChangesAsync();  // Save changes to delete previous duties

            // Shuffle the teachers randomly
            var random = new Random();
            var shuffledTeachers = teachers.OrderBy(t => random.Next()).ToList();

            // Track already assigned pairs of teachers for each paper
            var assignedPairs = new HashSet<string>();

            // Pre-generate teacher pairs for all papers before starting the assignment
            var teacherPairs = new List<Tuple<int, int>>();

            // Generate teacher pairs without any assignments yet
            for (int i = 0; i < shuffledTeachers.Count; i++)
            {
                for (int j = i + 1; j < shuffledTeachers.Count; j++)
                {
                    teacherPairs.Add(new Tuple<int, int>(shuffledTeachers[i].TeacherId, shuffledTeachers[j].TeacherId));
                }
            }

            // Loop through each paper and assign teachers in pairs randomly
            var paperAssignments = new List<Duty>();  // Store the duties to be added to the database
            foreach (var paper in papers)
            {
                // Convert DateOnly to DateTime (add midnight as the time part)
                DateTime paperDateTime = paper.Date.ToDateTime(TimeOnly.MinValue);  // Convert DateOnly to DateTime

                // Shuffle the teacher pairs to get random pairings
                var availablePairs = teacherPairs.OrderBy(t => random.Next()).ToList();

                foreach (var pair in availablePairs)
                {
                    // Check if this pair of teachers is already assigned to any paper on the same date
                    string pairKey = string.Join("-", pair.Item1, pair.Item2);  // Generate a unique key for the pair

                    if (!assignedPairs.Contains(pairKey))
                    {
                        // Create the first duty assignment
                        var duty1 = new Duty
                        {
                            TeacherId = pair.Item1,
                            RoomId = paper.RoomId,
                            Date = paper.Date,  // Store as DateOnly in the database
                            TimeSlot = paper.TimeSlot,
                            PaperId = paper.PaperId
                        };

                        // Create the second duty assignment
                        var duty2 = new Duty
                        {
                            TeacherId = pair.Item2,
                            RoomId = paper.RoomId,
                            Date = paper.Date,  // Store as DateOnly in the database
                            TimeSlot = paper.TimeSlot,
                            PaperId = paper.PaperId
                        };

                        // Add the duty assignments to the list (instead of adding directly to the database, batch them)
                        paperAssignments.Add(duty1);
                        paperAssignments.Add(duty2);

                        // Mark this pair as assigned
                        assignedPairs.Add(pairKey);

                        break; // Found a valid pair, break out of the loop
                    }
                }
            }

            // Add all the duties in batch to the database
            await _db.Duties.AddRangeAsync(paperAssignments);
            await _db.SaveChangesAsync();

            // Redirect back to the index page after generating duties
            return RedirectToAction("Index");
        }

    }
}


/*

public async Task<IActionResult> GenerateTeacherDuties()
{
    // Get all papers from the database
    var papers = await _db.Papers
        .Include(p => p.Room)
        .Include(p => p.Course)
        .ToListAsync();

    // Get all teachers from the database
    var teachers = await _db.Teachers.ToListAsync();

    // Ensure there are teachers available
    if (teachers.Count == 0)
    {
        ModelState.AddModelError("", "There are no teachers available.");
        return RedirectToAction("Index");
    }

    // Delete existing duties for the current papers
    var existingDuties = await _db.Duties
        .Where(d => papers.Select(p => p.PaperId).Contains(d.PaperId))
        .ToListAsync();

    _db.Duties.RemoveRange(existingDuties);  // Remove previous duties
    await _db.SaveChangesAsync();  // Save changes to delete previous duties

    // Shuffle the teachers randomly
    var random = new Random();
    var shuffledTeachers = teachers.OrderBy(t => random.Next()).ToList();

    // Track assigned teachers for each day to prevent multiple assignments
    var assignedTeachers = new Dictionary<DateTime, HashSet<int>>();

    // Loop through each paper and assign teachers in pairs randomly
    int teacherIndex = 0;  // To keep track of teacher assignments
    foreach (var paper in papers)
    {
        // Get two teachers for the current paper
        var firstTeacher = shuffledTeachers[teacherIndex];
        var secondTeacher = shuffledTeachers[(teacherIndex + 1) % shuffledTeachers.Count];  // Wrap around if necessary

        // Check if the first teacher is already assigned to a duty on the same date
        bool firstTeacherAssigned = assignedTeachers.ContainsKey(paper.Date) && 
                                    assignedTeachers[paper.Date].Contains(firstTeacher.TeacherId);

        // Check if the second teacher is already assigned to a duty on the same date
        bool secondTeacherAssigned = assignedTeachers.ContainsKey(paper.Date) && 
                                     assignedTeachers[paper.Date].Contains(secondTeacher.TeacherId);

        // If the teachers are already assigned on the same day, skip this pair and continue
        if (firstTeacherAssigned || secondTeacherAssigned)
        {
            teacherIndex = (teacherIndex + 2) % shuffledTeachers.Count;
            continue;
        }

        // Create the first duty assignment
        var duty1 = new Duty
        {
            TeacherId = firstTeacher.TeacherId,
            RoomId = paper.RoomId,
            Date = paper.Date,
            TimeSlot = paper.TimeSlot,
            PaperId = paper.PaperId
        };

        // Create the second duty assignment
        var duty2 = new Duty
        {
            TeacherId = secondTeacher.TeacherId,
            RoomId = paper.RoomId,
            Date = paper.Date,
            TimeSlot = paper.TimeSlot,
            PaperId = paper.PaperId
        };

        // Add the duty assignments to the database
        await _db.Duties.AddAsync(duty1);
        await _db.Duties.AddAsync(duty2);

        // Track the teachers' assignments on this date
        if (!assignedTeachers.ContainsKey(paper.Date))
        {
            assignedTeachers[paper.Date] = new HashSet<int>();
        }
        assignedTeachers[paper.Date].Add(firstTeacher.TeacherId);
        assignedTeachers[paper.Date].Add(secondTeacher.TeacherId);

        // Move to the next pair of teachers
        teacherIndex = (teacherIndex + 2) % shuffledTeachers.Count;  // Increment by 2 and wrap around if needed
    }

    // Save all the changes to the database
    await _db.SaveChangesAsync();

    // Redirect back to the index page after generating duties
    return RedirectToAction("Index");
}
 

 */