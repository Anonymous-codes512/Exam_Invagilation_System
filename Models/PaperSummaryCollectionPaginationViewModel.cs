namespace Exam_Invagilation_System.Models
{
    public class PaperSummaryCollectionPaginationViewModel
    {
        public List<PaperSummaryCollection> PaperSummaryCollection { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
    }

}
