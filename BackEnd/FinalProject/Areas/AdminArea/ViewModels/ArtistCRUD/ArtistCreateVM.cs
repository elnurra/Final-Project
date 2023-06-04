using System.ComponentModel.DataAnnotations;

namespace FinalProject.Areas.AdminArea.ViewModels.ArtistCRUD
{
    public class ArtistCreateVM
    {
        [Required, MaxLength(50)]
        public string Name { get; set; } = null!;
    }
}
