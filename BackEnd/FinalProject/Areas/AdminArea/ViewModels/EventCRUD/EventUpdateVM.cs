using FinalProject.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Areas.AdminArea.ViewModels.EventCRUD
{
    public class EventUpdateVM
    {
        [Required(ErrorMessage = "Should be not empty")]
        public IFormFile Photo { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string AddressUrl { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Address { get; set; } = null!;
        public DateTime CreatedTime { get; set; }
        public int ArtistId { get; set; }
        public Artist Artist { get; set; } = null!;
        [Column(TypeName = "decimal(18,0)")]
        public decimal Price { get; set; }
        public List<Comment> Comments { get; set; } = null!;
    }
}
