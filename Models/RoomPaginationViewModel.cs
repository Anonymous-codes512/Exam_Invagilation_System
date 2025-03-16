namespace Exam_Invagilation_System.Models
{
    internal class RoomPaginationViewModel
    {
        public List<Room> Rooms { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
    }
}