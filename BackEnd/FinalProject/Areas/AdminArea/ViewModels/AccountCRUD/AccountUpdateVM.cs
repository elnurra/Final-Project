using FinalProject.Models;

namespace FinalProject.Areas.AdminArea.ViewModels.AccountCRUD
{
    public class AccountUpdateVM
    {
        public IFormFile Photo { get; set; } = null!;
        public string Fullname { get; set; } = null!;
        public string? UserName { get; set; } = null!;
        public string? Email { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
    }
}
