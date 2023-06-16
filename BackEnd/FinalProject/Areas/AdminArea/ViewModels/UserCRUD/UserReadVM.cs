using FinalProject.Models;

namespace FinalProject.Areas.AdminArea.ViewModels.UserCRUD
{
    public class UserReadVM:BaseVM
    {
        public List<AppUser> AppUsers { get; set; } = null!;
    }
}
