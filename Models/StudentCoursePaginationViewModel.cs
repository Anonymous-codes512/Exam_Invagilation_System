namespace Exam_Invagilation_System.Models
{
    public class StudentCoursePaginationViewModel
    {
        // List of student-course registrations (now using StudentCourseViewModel)
        public List<StudentCourseViewModel> StudentCourses { get; set; }

        // List of students for the dropdown or filtering
        public List<Student> Students { get; set; }

        // List of courses for the dropdown or filtering
        public List<Course> Courses { get; set; }

        // Pagination information
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }

        // Search filters (e.g., for filtering registrations)
        public string RegistrationNumber { get; set; }
        public string CourseCode { get; set; }
    }
}
