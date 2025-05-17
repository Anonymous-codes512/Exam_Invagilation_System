namespace Exam_Invagilation_System.Models
{
    public class AttendancePaginationViewModel
    {
        public List<Attendance> Attendances { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
    }

}
