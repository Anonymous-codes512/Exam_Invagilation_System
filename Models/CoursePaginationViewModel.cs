namespace Exam_Invagilation_System.Models
{
    internal class CoursePaginationViewModel
    {
        public List<Course> Courses { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public List<Teacher> Teachers { get; set; }

        // 🔹 Add this property to fix the issue
        public string TeacherEmployeeNumber { get; set; }
    }
}
