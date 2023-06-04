using System.ComponentModel.DataAnnotations;

namespace FinalProject.Areas.AdminArea.ViewModels.GenreCRUD
{
    public class GenreCreateVM
    {
        [Required, MaxLength(50)]
        public string Name { get; set; } = null!;
    }
}
