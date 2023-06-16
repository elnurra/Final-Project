using FinalProject.Models;

namespace FinalProject.Areas.AdminArea.ViewModels.AlbumCRUD
{
    public class AlbumUpdateVM
    {
        public IFormFile Photo { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public string Name { get; set; } = null!;
        public DateTime CreatedTime { get; set; }
        public string Label { get; set; } = null!;
        public int ArtistId { get; set; }
        public Artist Artist { get; set; } = null!;
        public int GenreId { get; set; }
        public Genre Genre { get; set; } = null!;
        public List<Song> Songs { get; set; } = null!;
        public List<Comment> Comments { get; set; } = null!;
        public bool IsDeleted { get; set; }


    }
}
