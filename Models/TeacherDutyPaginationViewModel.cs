namespace Exam_Invagilation_System.Models
{
    internal class TeacherDutyPaginationViewModel
    {
        public List<Duty> TeacherDuties { get; set; }
        public List<Teacher> Teachers { get; set; }
        public List<Room> Rooms { get; set; }
        public List<Paper> Papers { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
    }
}
