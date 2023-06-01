using System.ComponentModel.DataAnnotations;

namespace FinalProject.ViewModels.AccountVM
{
    public class ForgotPasswordVM
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;
    }
}
