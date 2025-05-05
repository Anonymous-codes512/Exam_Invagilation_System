namespace Exam_Invagilation_System.Models
{
    public class StudentCourseViewModel
    {
        public string RegistrationNumber { get; set; }
        public string StudentName { get; set; }

        public List<CourseViewModel> Courses { get; set; }
    }

    public class CourseViewModel
    {
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string CourseAttendance { get; set; }
        public string TeacherName { get; set; }  // New field for Teacher's Name
        public string TeacherEmployeeNumber { get; set; }  // New field for Teacher's Employee Number
    }


}