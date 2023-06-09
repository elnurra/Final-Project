using FinalProject.Models.BaseClass;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class Comment : BaseEntity
    {
        public AppUser User { get; set; } = null!;
        public string AppUserId { get; set; } = null!;
        public Album? Album { get; set; }
        public int? AlbumId { get; set; }
        public Event? Event { get; set; }
        public int? EventId { get; set; }
        public DateTime CreatedTime { get; set; }
        [Required]
        public string Content { get; set; } = null!;


    }
}
