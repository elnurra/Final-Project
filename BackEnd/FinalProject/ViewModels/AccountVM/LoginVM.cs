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
        [Required]
        public string Token { get; set; } = null!;
        public string SiteKey { get; set; } = null!;


    }
}
