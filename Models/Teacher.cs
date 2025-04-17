using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Exam_Invagilation_System.Models
{
    public class Teacher
    {
        [Key]
        public int TeacherId { get; set; }

        [Required(ErrorMessage = "Registration Number is required")]
        public required String TeacherEmployeeNumber { get; set; }

        [Required(ErrorMessage = "Teacher Name is required")]
        public required string TeacherName { get; set; }

        [Required(ErrorMessage = "Teacher Email is required")]
        public required string TeacherEmail { get; set; }

        [Required(ErrorMessage = "Teacher Designation is required")]
        public required string TeacherDesignation { get; set; }

        [Required(ErrorMessage = "Teacher Department is required")]
        public required string TeacherDepartment { get; set; }

        [JsonIgnore] // 👈 Prevents circular reference
        public ICollection<Course> Courses { get; set; } = new List<Course>();
        
    }
}
