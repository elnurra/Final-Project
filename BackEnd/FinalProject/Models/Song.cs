using FinalProject.Models.BaseClass;

namespace FinalProject.Models
{
    public class Song:BaseEntity
    {
        public string Name { get; set; } = null!;
        public int AlbumId { get; set; } 
        public Album Album { get; set; } = null!;
        public string SongUrl { get; set; } = null!;
        public List<AppUser> Users { get; set; } = null!;

    }
}
