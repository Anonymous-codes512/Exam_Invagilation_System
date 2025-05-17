using Exam_Invagilation_System.Entities;
using Exam_Invagilation_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Exam_Invagilation_System.Controllers
{
    [Authorize(AuthenticationSchemes = "MyCookieAuth")]

    public class SeatingPlanController : Controller
    {
        private readonly AppDbContext _db;

        public SeatingPlanController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult Index(int pageNumber = 1, int pageSize = 30)
        {
            // Eager load SittingArrangement with related entities
            var seatingArrangements = _db.SittingArrangements
                .Include(s => s.Room)
                .Include(s => s.Paper)
                    .ThenInclude(p => p.Course)
                .OrderBy(s => s.Room.RoomNumber)
                .ThenBy(s => s.Paper.TimeSlot)
                .ThenBy(s => s.Seat)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Get total count for pagination
            var totalArrangements = _db.SittingArrangements.Count();

            // Calculate total pages
            var totalPages = (int)Math.Ceiling((double)totalArrangements / pageSize);

            // Create ViewModel
            var model = new SeatingArrangementPaginationViewModel
            {
                SeatingArrangements = seatingArrangements,
                CurrentPage = pageNumber,
                TotalPages = totalPages,
                PageSize = pageSize,
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult GenerateSeatingPlan()
        {
            // Fetch all exam papers with related data
            var papers = _db.Papers
                .Include(p => p.Room)
                .Include(p => p.Course)
                .OrderBy(p => p.Date)
                .ThenBy(p => p.TimeSlot)
                .ToList();

            // Group papers by room and time slot
            var roomTimeSlots = papers
                .Select(p => new { p.Room, p.TimeSlot })
                .Distinct()
                .ToList();

            // Get students for each course
            var studentsPerCourse = papers
                .GroupBy(p => p.Course.CourseCode)
                .ToDictionary(
                    g => g.Key,
                    g => _db.StudentCourses
                        .Where(sc => sc.CourseCode == g.Key)
                        .Select(sc => sc.Student)
                        .OrderBy(s => s.RegistrationNumber)
                        .ToList()
                );

            var seatingArrangements = new List<SittingArrangement>();

            // Process each room-time slot combination
            foreach (var roomTimeSlot in roomTimeSlots)
            {
                var room = roomTimeSlot.Room;
                var timeSlot = roomTimeSlot.TimeSlot;
                int totalRows = room.Rows;
                int totalColumns = room.Columns;

                // Get papers for this room and time slot
                var currentPapers = papers
                    .Where(p => p.RoomId == room.RoomId && p.TimeSlot == timeSlot)
                    .OrderBy(p => p.Course.CourseCode)
                    .ToList();

                // Special handling for exactly two courses
                if (currentPapers.Count == 2)
                {
                    ProcessTwoCourses(room, timeSlot, totalRows, totalColumns,
                                    currentPapers, studentsPerCourse, seatingArrangements);
                }
                else
                {
                    ProcessMultipleCourses(room, timeSlot, totalRows, totalColumns,
                                        currentPapers, studentsPerCourse, seatingArrangements);
                }
            }

            // Group and order the seating arrangements for output
            var orderedSeating = seatingArrangements
                .OrderBy(s => s.Room.RoomNumber)
                .ThenBy(s => s.Paper.TimeSlot)
                .ThenBy(s => s.Paper.Course.CourseCode)
                .ThenBy(s => s.Seat)
                .ToList();

            //Console.WriteLine("Generated Seating Plan:");
            //foreach (var arrangement in seatingArrangements
            //    .OrderBy(s => s.Room.RoomNumber)
            //    .ThenBy(s => s.Seat))
            //{
            //    Console.WriteLine($"Room: {arrangement.Room.RoomNumber}, " +
            //                     $"Seat: {arrangement.Seat}, " +
            //                     $"Student: {arrangement.RegistrationNumber}, " +
            //                     $"Course: {arrangement.Paper.Course.CourseCode}, " +
            //                     $"TimeSlot: {arrangement.Paper.TimeSlot}");
            //}

            // Save to database
            _db.SittingArrangements.AddRange(orderedSeating);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        private void ProcessTwoCourses(Room room, string timeSlot, int totalRows, int totalColumns,
            List<Paper> papers, Dictionary<string, List<Student>> studentsPerCourse,
            List<SittingArrangement> seatingArrangements)
        {
            var course1 = papers[0];
            var course2 = papers[1];
            var students1 = studentsPerCourse[course1.Course.CourseCode];
            var students2 = studentsPerCourse[course2.Course.CourseCode];

            int currentColumn = 0;
            int studentIndex1 = 0;
            int studentIndex2 = 0;

            while (studentIndex1 < students1.Count || studentIndex2 < students2.Count)
            {
                // Assign course1 students to even columns (0, 2, 4... => A, C, E...)
                if (studentIndex1 < students1.Count)
                {
                    int col = currentColumn;
                    if (col < totalColumns)
                    {
                        AssignStudentsToColumn(room, timeSlot, totalRows, course1,
                                            students1, ref studentIndex1, col, seatingArrangements);
                    }
                }

                // Assign course2 students to odd columns (1, 3, 5... => B, D, F...)
                if (studentIndex2 < students2.Count)
                {
                    int col = currentColumn + 1;
                    if (col < totalColumns)
                    {
                        AssignStudentsToColumn(room, timeSlot, totalRows, course2,
                                            students2, ref studentIndex2, col, seatingArrangements);
                    }
                }

                currentColumn += 2;
            }
        }

        private void ProcessMultipleCourses(Room room, string timeSlot, int totalRows, int totalColumns,
            List<Paper> papers, Dictionary<string, List<Student>> studentsPerCourse,
            List<SittingArrangement> seatingArrangements)
        {
            var columnAssignments = new Dictionary<int, string>();
            int currentColumn = 0;

            foreach (var paper in papers)
            {
                string courseCode = paper.Course.CourseCode;
                var students = studentsPerCourse[courseCode];
                int studentIndex = 0;

                // Find appropriate column for this course
                int assignedColumn = FindAvailableColumn(papers, columnAssignments, courseCode, currentColumn, totalColumns);
                columnAssignments[assignedColumn] = courseCode;
                currentColumn = assignedColumn + 1;

                // Assign students to seats in this column
                while (studentIndex < students.Count)
                {
                    if (assignedColumn >= totalColumns) break;

                    AssignStudentsToColumn(room, timeSlot, totalRows, paper,
                                        students, ref studentIndex, assignedColumn, seatingArrangements);

                    // Move to next non-adjacent column for same course
                    assignedColumn += 2;
                    while (assignedColumn < totalColumns && columnAssignments.ContainsKey(assignedColumn))
                    {
                        assignedColumn++;
                    }
                }
            }
        }

        private void AssignStudentsToColumn(Room room, string timeSlot, int totalRows, Paper paper,
            List<Student> students, ref int studentIndex, int column, List<SittingArrangement> seatingArrangements)
        {
            // Assign up to 7 students in this column
            for (int row = 1; row <= totalRows && studentIndex < students.Count; row++)
            {
                string seatLabel = $"{(char)('A' + column)}{row}";

                seatingArrangements.Add(new SittingArrangement
                {
                    RoomId = room.RoomId,
                    Seat = seatLabel,
                    RegistrationNumber = students[studentIndex].RegistrationNumber,
                    PaperId = paper.PaperId,
                    Paper = paper,
                    Room = room
                });

                studentIndex++;
            }
        }

        private int FindAvailableColumn(List<Paper> papers, Dictionary<int, string> columnAssignments,
            string courseCode, int startColumn, int totalColumns)
        {
            // Try to find an available column that's not adjacent to same course
            for (int col = startColumn; col < totalColumns; col++)
            {
                if (!columnAssignments.ContainsKey(col))
                {
                    // Check adjacent columns
                    bool adjacentConflict = false;
                    if (col > 0 && columnAssignments.ContainsKey(col - 1) &&
                        columnAssignments[col - 1] == courseCode)
                        adjacentConflict = true;

                    if (col < totalColumns - 1 && columnAssignments.ContainsKey(col + 1) &&
                        columnAssignments[col + 1] == courseCode)
                        adjacentConflict = true;

                    if (!adjacentConflict)
                        return col;
                }
            }

            // If no perfect column found, just find any available
            for (int col = 0; col < totalColumns; col++)
            {
                if (!columnAssignments.ContainsKey(col))
                    return col;
            }

            throw new Exception("No available columns in room");
        }
    }
    }
