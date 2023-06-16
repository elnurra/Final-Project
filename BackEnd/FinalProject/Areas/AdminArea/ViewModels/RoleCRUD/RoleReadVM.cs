using Microsoft.AspNetCore.Identity;

namespace FinalProject.Areas.AdminArea.ViewModels.RoleCRUD
{
    public class RoleReadVM:BaseVM
    {
       public List<IdentityRole> IdentityRoles { get; set; } = null!;
        
    }
}
