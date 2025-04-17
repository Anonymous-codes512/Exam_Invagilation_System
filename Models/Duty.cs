using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exam_Invagilation_System.Models
{
    public class Duty
    {
        [Key]
        public int DutyId { get; set; }

        // Foreign Key for Teacher
        [Required]
        [ForeignKey(nameof(Teacher))]
        public int TeacherId { get; set; }

        // Navigation property for Teacher
        public Teacher Teacher { get; set; }

        // Foreign Key for Room
        [Required]
        [ForeignKey(nameof(Room))]
        public int RoomId { get; set; }

        // Navigation property for Room
        public Room Room { get; set; }
        
        // Foreign Key for Room
        [Required]
        [ForeignKey(nameof(Paper))]
        public int PaperId { get; set; }

        // Navigation property for Room
        public Paper Paper { get; set; }

        // The Date of the Paper
        [Required]
        public DateOnly Date { get; set; }

        // Time Slot of the exam (e.g. 9 AM - 12 PM)
        [Required]
        [StringLength(50)]
        public string TimeSlot { get; set; }
    }
}
