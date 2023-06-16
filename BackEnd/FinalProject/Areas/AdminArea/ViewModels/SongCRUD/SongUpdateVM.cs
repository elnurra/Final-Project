using FinalProject.Models;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Areas.AdminArea.ViewModels.SongCRUD
{
    public class SongUpdateVM
    {
        [Required(ErrorMessage = "Should be not empty")]
        public IFormFile Audio { get; set; } = null!;
        public string SongUrl = null!;
        [Required(ErrorMessage = "Should be not empty")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Should be not empty")]
        public int AlbumId { get; set; }
        public Album Album { get; set; } = null!;
        public bool IsDeleted { get; set; }
    }
}
