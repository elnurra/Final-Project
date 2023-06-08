using FinalProject.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Areas.AdminArea.ViewModels.UserCRUD
{
    public class UserCreateVM
    {
        public List<IdentityRole>? AllRoles { get; set; }
        public string Fullname { get; set; } = null!;
        [Required]
        public string Username { get; set; } = null!;
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        [Required]
        [DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = null!;
    }
}
