using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
namespace Exam_Invagilation_System.Entities
{
    [Index(nameof(Email), IsUnique = true)]
    public class UserAccount
    {
        [Key]
        public int Id { get; set; }

        [Required (ErrorMessage = "Name is required.")]
        required
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [DataType(DataType.EmailAddress)]
        required
        public string Email {  get; set; }

        [Required(ErrorMessage = "Password is required.")]
        required
        public string Password { get; set; }
    }
}
