namespace Exam_Invagilation_System.Models
{
    internal class TeacherPaginationViewModel
    {
        public List<Teacher> Teachers { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
    }
}