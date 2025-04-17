using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exam_Invagilation_System.Models
{
    public class Paper
    {
        // Primary key for the Paper table
        public int PaperId { get; set; }

        // Foreign Key for Course (with a custom error message in case of missing value)
        [Required(ErrorMessage = "CourseCode is required.")]
        [ForeignKey(nameof(Course))]
        public string CourseCode { get; set; }

        // Navigation property for Course entity
        public Course Course { get; set; }

        // Foreign Key for Room (with a custom error message in case of missing value)
        [Required(ErrorMessage = "RoomId is required.")]
        [ForeignKey(nameof(Room))]
        public int RoomId { get; set; }

        // Navigation property for Room entity
        public Room Room { get; set; }

        // Required field: Date of the exam (with validation)
        [Required(ErrorMessage = "Date is required.")]
        public DateOnly Date { get; set; }

        // Required field: Time slot of the exam (with validation)
        [Required(ErrorMessage = "TimeSlot is required.")]
        [StringLength(50, ErrorMessage = "TimeSlot cannot be longer than 50 characters.")]
        public string TimeSlot { get; set; }

        // Required field: Term of the exam (with validation)
        [Required(ErrorMessage = "ExamTerm is required.")]
        [StringLength(50, ErrorMessage = "ExamTerm cannot be longer than 50 characters.")]
        public string ExamTerm { get; set; }
    }
}
