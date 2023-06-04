using System.ComponentModel.DataAnnotations;

namespace FinalProject.Areas.AdminArea.ViewModels.SliderCRUD
{
    public class SliderCreateVM
    {
        [Required(ErrorMessage = "Should be not empty")]
        public IFormFile Photo { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
