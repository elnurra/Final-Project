using System.ComponentModel.DataAnnotations;

namespace FinalProject.ViewModels.AccountVM
{
    public class LoginVM
    {
        [Required]
        public string UserNameorEmail { get; set; } = null!;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
