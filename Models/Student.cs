using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Exam_Invagilation_System.Models
{
    public class Student
    {
        [Key] // Primary Key
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Registration Number is required")]
        public string? RegistrationNumber { get; set; }

        [Required(ErrorMessage = "Student Name is required")]
        public string? StudentName { get; set; }

        [Required(ErrorMessage = "Father Name is required")]
        public string? FatherName { get; set; }

        [Required(ErrorMessage = "Program is required")]
        public string? Program { get; set; }

        [Required(ErrorMessage = "Semester is required")]
        public int Semester { get; set; }

        [Required(ErrorMessage = "Section is required")]
        public string? Section { get; set; }

        [Range(0, 100, ErrorMessage = "Attendance must be between 0 and 100")]
        public int Attendance { get; set; }

        [Required(ErrorMessage = "Dues status is required")]
        public bool DuesClear { get; set; }

        public ICollection<StudentCourse>? StudentCourses { get; set; } = new List<StudentCourse>();
    }

}
