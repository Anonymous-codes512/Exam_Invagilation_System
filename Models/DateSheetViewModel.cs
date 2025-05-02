public class DateSheetViewModel
{
    public DateOnly Date { get; set; }  // The date for the papers
    public List<PaperViewModel> Papers { get; set; }  // List of papers for that date
}
