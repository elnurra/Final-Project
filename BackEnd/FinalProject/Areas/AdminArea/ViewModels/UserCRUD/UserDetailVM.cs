using FinalProject.Models;
using Microsoft.AspNetCore.Identity;

namespace FinalProject.Areas.AdminArea.ViewModels.UserCRUD
{
    public class UserDetailVM
    {
        public AppUser User { get; set; } = null!;
        public IList<string> UserRoles { get; set; } = null!;
        public List<IdentityRole> AllRoles { get; set; } = null!;
    }
}
