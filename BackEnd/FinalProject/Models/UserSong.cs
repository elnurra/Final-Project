using FinalProject.Models.BaseClass;

namespace FinalProject.Models
{
    public class UserSong:BaseEntity
    {
        public int SongId { get; set; } 
        public Song Song { get; set; } = null!;
        public string AppUserId { get; set; } = null!;
        public AppUser User { get; set; } = null!;  
    }
}
