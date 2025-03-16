namespace Exam_Invagilation_System.Models
{
    public class Paper
    {
        public int PaperId { get; set; }
        public string CourseCode { get; set; }
        public int RoomId { get; set; }
        public DateOnly Date { get; set; }
        public string TimeSlot { get; set; }
        public string ExamTerm { get; set; }

    }
}
