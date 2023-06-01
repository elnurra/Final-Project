using System.ComponentModel.DataAnnotations;

namespace FinalProject.ViewModels.AccountVM
{
    public class ResetPasswordVM
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        [Required]
        [DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string Token { get; set; } = null!;
    }
}
