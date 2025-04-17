using System.ComponentModel.DataAnnotations.Schema;

namespace Exam_Invagilation_System.Models
{
    public class StudentCourse
    {
        public string RegistrationNumber { get; set; }
        public Student Student { get; set; }

        [ForeignKey(nameof(CourseCode))]
        public string CourseCode { get; set; }
        public Course Course { get; set; }
    }
}
