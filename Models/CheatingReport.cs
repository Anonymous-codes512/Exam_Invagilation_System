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

        // Foreign Key for Room using RoomNumber (string)
        [ForeignKey("Room")]
        public string RoomNumber { get; set; }
        public Room Room { get; set; }

        // Foreign Key for Course using CourseCode
        [ForeignKey("Course")]
        public string CourseCode { get; set; }
        public Course Course { get; set; }

        public DateOnly Date { get; set; }
        public string TimeSlot { get; set; }

        // Make Description nullable
        public string? Description { get; set; }  // Allow null values here

        // Additional fields for the report
        public string UnfairType { get; set; }
        public string OtherDetails { get; set; }
        public string IncidentDetails { get; set; }
        public string Status { get; set; }
    }

}
