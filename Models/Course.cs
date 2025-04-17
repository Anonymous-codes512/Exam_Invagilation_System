using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Exam_Invagilation_System.Models;

public class Course
{
    [Key]
    public int CourseId { get; set; }

    [Required(ErrorMessage = "Course Code is Required")]
    public required string CourseCode { get; set; }

    [Required(ErrorMessage = "Course Name is Required")]
    public required string CourseName { get; set; }

    [Required(ErrorMessage = "Course PreRequisite is Required")]
    public required string PreRequisite { get; set; }

    [Required(ErrorMessage = "Teacher is Required")]
    [ForeignKey(nameof(Teacher))]
    public required string TeacherEmployeeNumber { get; set; }

    [JsonIgnore] // 👈 Prevents circular reference
    public Teacher Teacher { get; set; }

    [JsonIgnore] // 👈 Prevents circular reference
    public ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
}
