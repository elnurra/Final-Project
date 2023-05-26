using FinalProject.Models;

namespace FinalProject.ViewModels
{
    public class AlbumVM
    {
        public List<Genre> Genres { get; set; } = null!;
        public List<Album> Albums { get; set; } = null!;
    }
}
