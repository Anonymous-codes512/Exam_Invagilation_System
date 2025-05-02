namespace Exam_Invagilation_System.Models
{
    public class SeatingArrangementPaginationViewModel
    {
        public List<SittingArrangement> SeatingArrangements { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
    }
}
