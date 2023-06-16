using FinalProject.Models.BaseClass;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class Contact:BaseEntity
    {
        [Required(ErrorMessage = "Name field is required")]
        public string Name { get; set; } = null!;
        [EmailAddress(ErrorMessage = "Email field is required")]
        public string Email { get; set; } = null!;
        [Phone(ErrorMessage = "Phone number is required")]
        public int Number { get; set; }
        [Required(ErrorMessage ="Content field is required")]
        public string Content { get; set; } = null!;
    }
}
