using Exam_Invagilation_System.Models;

public class DateSheetPaginationViewModel
{
    public List<DateSheetViewModel> DateSheets { get; set; }  // List of grouped papers by Date
    public List<Course> Courses { get; set; }  // List of courses (for filtering)
    public List<Room> Rooms { get; set; }  // List of rooms (for filtering)
    public int CurrentPage { get; set; }  // Current page number
    public int TotalPages { get; set; }  // Total number of pages
    public int PageSize { get; set; }  // Number of items per page
    public string RegistrationNumber { get; set; }  // Optional filter for registration number
    public string CourseCode { get; set; }  // Optional filter for course code
}
