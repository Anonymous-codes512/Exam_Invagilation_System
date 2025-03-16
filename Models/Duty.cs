namespace Exam_Invagilation_System.Models
{
    public class Duty
    {
        public int DutyId { get; set; }
        public int TeacherId { get; set; }
        public int RoomId { get; set; }
        public DateOnly Date { get; set; }
        public string TimeSlot { get; set; }
    }
}
