using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Areas.AdminArea.ViewModels.RoleCRUD
{
    public class RoleUpdateVM
    {
        [Required]
        public string Name { get; set; } = null!;
    }
}
