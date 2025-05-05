using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
namespace Exam_Invagilation_System.Models;

public class StudentCourse
{
    public string RegistrationNumber { get; set; }
    
    [JsonIgnore]
    public Student Student { get; set; }

    public string CourseAttendance { get; set; }


    [ForeignKey(nameof(CourseCode))]
    public string CourseCode { get; set; }
    public Course Course { get; set; }
}
