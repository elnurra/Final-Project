using FinalProject.Models;

namespace FinalProject.Areas.AdminArea.ViewModels
{
    public class DashboardVM
    {
        public List<Genre> Genres { get; set; } = null!;
        public List<Artist> Artists { get; set; } = null!;
        public List<Album> Albums { get; set; } = null!;
        public List<Song> Songs { get; set; } = null!;
    }
}
