namespace Exam_Invagilation_System.Models
{
    public class DateSheetViewModel
    {
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public int RoomId { get; set; }
        public string RoomNumber { get; set; }
        public string Location { get; set; }
        public string ExamTerm  { get; set; }
        public DateOnly Date  { get; set; }
        public string TimeSlot { get; set; }

    }

}
