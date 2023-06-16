using FinalProject.Models;

namespace FinalProject.Areas.AdminArea.ViewModels.ArtistCRUD
{
    public class ArtistReadVM:BaseVM
    {
        public List<Artist> Artists { get; set; } = null!;

    }
}
