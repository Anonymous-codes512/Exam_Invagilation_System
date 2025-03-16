using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exam_Invagilation_System.Models
{
    [Index(nameof(CourseCode), IsUnique = true)]  // Unique Key
    public class Course
    {
        [Key]
        public int CourseId { get; set; }  // Primary Key

        [Required(ErrorMessage = "Course Code is Required")]
        public required string CourseCode { get; set; }  // Secondary Key

        [Required(ErrorMessage = "Course Name is Required")]
        public required string CourseName { get; set; }

        [Required(ErrorMessage = "Course PreRequisite is Required")]
        public required string PreRequisite { get; set; }

        public ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
    }
}
