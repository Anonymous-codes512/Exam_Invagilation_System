using Exam_Invagilation_System.Models;

public class Attendance
{
    public int AttendanceId { get; set; }

    // Foreign Key for Student
    public string RegistrationNumber { get; set; }
    public virtual Student Student { get; set; }

    // Foreign Key for Teacher
    public string TeacherEmployeeNumber { get; set; }
    public virtual Teacher Teacher { get; set; }
    
    // Foreign Key for Room
    public string RoomNumber { get; set; }
    public virtual Room Room { get; set; }

    // Foreign Key for Paper
    public int PaperId { get; set; }
    public virtual Paper Paper { get; set; }

    public DateOnly Date { get; set; }
    public string TimeSlot { get; set; }
    public string Status { get; set; }
}
