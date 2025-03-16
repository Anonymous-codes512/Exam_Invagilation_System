using System.ComponentModel.DataAnnotations;

namespace Exam_Invagilation_System.Models
{
    public class Room
    {
        [Key]
        public int RoomId { get; set; }

        [Required(ErrorMessage = "Room Number is required")]
        public required string RoomNumber { get; set; }

        [Required(ErrorMessage = "Room Location is required")]
        public required string Location { get; set; }

        [Required(ErrorMessage = "Columns in room are required")]
        public int Columns { get; set; }

        [Required(ErrorMessage = "Rows in room are required")]
        public int Rows { get; set; }

        [Required(ErrorMessage = "Total Seeting capacity is required")]
        public int TotalStrength { get; set; }
    }
}
