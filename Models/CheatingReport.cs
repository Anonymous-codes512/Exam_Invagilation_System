using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exam_Invagilation_System.Models
{
    public class CheatingReport
    {
        public int CheatingReportId { get; set; }

        // Foreign Key for Student using RegistrationNumber
        [ForeignKey("Student")]
        public string RegistrationNumber { get; set; }
        public Student Student { get; set; }

        // Foreign Key for Teacher using TeacherEmployeeNumber
        [ForeignKey("Teacher")]
        public string TeacherEmployeeNumber { get; set; }
        public Teacher Teacher { get; set; }

        public int RoomId { get; set; }
        public string CourseCode { get; set; }
        public DateOnly Date { get; set; }
        public string TimeSlot { get; set; }
        public string Description { get; set; }
    }
}
