using System.ComponentModel.DataAnnotations;

namespace FinalProject.Areas.AdminArea.ViewModels.ArtistCRUD
{
    public class ArtistUpdateVM
    {
        [Required, MaxLength(50)]
        public string Name { get; set; } = null!;
        public bool IsDeleted { get; set; }
    }
}
