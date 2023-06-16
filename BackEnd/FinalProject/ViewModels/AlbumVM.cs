using FinalProject.Models;

namespace FinalProject.ViewModels
{
    public class AlbumVM
    {

        public int PageCount { get; set; }
        public int CurrentPage { get; set; }
        public List<Genre> Genres { get; set; } = null!;
        public Album Album { get; set; } = null!;
        public List<Album> Albums { get; set; } = null!;
        public List<Song> Songs { get; set; } = null!;
        public int SongId { get; set; }

    }
}
