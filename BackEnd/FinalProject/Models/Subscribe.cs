using FinalProject.Models.BaseClass;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class Subscribe:BaseEntity
    {
        [Required,MaxLength(100)]
        public string Email { get; set; } = null!;
    }

}
