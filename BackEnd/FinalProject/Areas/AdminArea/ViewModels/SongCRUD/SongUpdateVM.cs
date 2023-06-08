using FinalProject.Models;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Areas.AdminArea.ViewModels.SongCRUD
{
    public class SongUpdateVM
    {
        [Required(ErrorMessage = "Should be not empty")]
        public IFormFile Audio { get; set; } = null!;
        public string SongUrl = null!;
        public string Name { get; set; } = null!;
        public int AlbumId { get; set; }
        public Album Album { get; set; } = null!;
    }
}
