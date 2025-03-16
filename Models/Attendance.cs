namespace Exam_Invagilation_System.Models
{
    public class Attendance
    {
        public int AttendanceId { get; set; }
        public string RegistrationNumber { get; set; }
        public int TeacherId { get; set; }
        public int RoomId { get; set; }
        public string PaperId { get; set; }
        public string AnswerBook { get; set; }
        public int AdditionalSheet { get; set; }
        public DateTime Date { get; set; }
        public string TimeSlot { get; set; }
        public string Status { get; set; }
    }
}
