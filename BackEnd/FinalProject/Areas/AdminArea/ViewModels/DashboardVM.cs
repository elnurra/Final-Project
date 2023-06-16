using FinalProject.Models;
using Microsoft.AspNetCore.Identity;

namespace FinalProject.Areas.AdminArea.ViewModels
{
    public class DashboardVM
    {
        public List<Genre> Genres { get; set; } = null!;
        public List<Artist> Artists { get; set; } = null!;
        public List<Album> Albums { get; set; } = null!;
        public List<Song> Songs { get; set; } = null!;
        public List<Slider> Sliders { get; set; } = null!;
        public List<AppUser> AppUsers { get; set; } = null!;
        public List<Event> Events { get; set; } = null!;
        public List<IdentityRole> IdentityRoles { get; set; } = null!;
    }
}
