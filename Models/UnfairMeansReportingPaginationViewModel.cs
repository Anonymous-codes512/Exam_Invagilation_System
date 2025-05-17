namespace Exam_Invagilation_System.Models
{
    public class UnfairMeansReportingPaginationViewModel
    {
        public List<CheatingReport> UnfairCases { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
    }

}
