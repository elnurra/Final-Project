using FinalProject.Models;

namespace FinalProject.Areas.AdminArea.ViewModels.SliderCRUD
{
    public class SliderReadVM:BaseVM
    {
        public List<Slider> Sliders { get; set; } = null!;
    }
}
