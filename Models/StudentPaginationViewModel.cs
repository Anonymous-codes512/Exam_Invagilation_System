namespace Exam_Invagilation_System.Models
{
    public class StudentPaginationViewModel
    {
        public List<Student> Students { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
    }
}
