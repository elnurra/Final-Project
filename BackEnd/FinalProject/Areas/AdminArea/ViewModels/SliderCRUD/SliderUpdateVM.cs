using System.ComponentModel;

namespace FinalProject.Areas.AdminArea.ViewModels.SliderCRUD
{
    public class SliderUpdateVM
    {
        public IFormFile Photo { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsDeleted { get; set; }
    }
}
