namespace Exam_Invagilation_System.Models
{
    public class DateSheetPaginationViewModel
    {
        public List<Paper> DateSheets { get; set; }
        public List<Course> Courses { get; set; }
        public List<Room> Rooms { get; set; }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
    }

}
