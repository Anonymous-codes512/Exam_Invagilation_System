using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exam_Invagilation_System.Models
{
    public class PaperSummaryCollection
    {
        public int PaperSummaryCollectionId { get; set; } // Primary Key

        // Foreign Key for Teacher using TeacherEmployeeNumber
        [ForeignKey("Teacher")]
        public string TeacherEmployeeNumber { get; set; }
        public Teacher Teacher { get; set; }

        // The selected paper for summary collection
        public string SelectedPaper { get; set; }

        public string roomnumber { get; set; }

        // Total number of papers for the selected course or exam
        public int TotalPapers { get; set; }

        // Number of papers collected by the collector
        public int NumberOfPapers { get; set; }

        // Name of the collector who collected the papers
        public string CollectorName { get; set; }

        // Optional: Timestamp of when the summary collection was recorded
        public DateTime CollectedDate { get; set; } = DateTime.Now;
    }
}
