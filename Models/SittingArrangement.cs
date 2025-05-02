using Exam_Invagilation_System.Models;

public class SittingArrangement
{
    public int SittingArrangementId { get; set; }

    // Foreign Key for Room
    public int RoomId { get; set; }
    public Room Room { get; set; }

    // Foreign Key for Student
    public string RegistrationNumber { get; set; }
    public Student Student { get; set; }

    // Foreign Key for Paper (exam)
    public int PaperId { get; set; }
    public Paper Paper { get; set; }

    // Seat assigned to the student
    public string Seat { get; set; }
}
