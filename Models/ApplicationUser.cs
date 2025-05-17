using Microsoft.AspNetCore.Identity;

namespace Exam_Invagilation_System.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
