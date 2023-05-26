using FinalProject.Models.BaseClass;

namespace FinalProject.Models
{
    public class Album:BaseEntity
    {
        public string Name { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public DateTime CreatedTime { get; set; }
        public string Label { get; set; } = null!;
        public int ArtistId { get; set; } 
        public Artist Artist { get; set; } = null!;
        public int GenreId { get; set; } 
        public Genre Genre { get; set; } = null!;
        public List<Song> Songs { get; set; } = null!;

    }
}
