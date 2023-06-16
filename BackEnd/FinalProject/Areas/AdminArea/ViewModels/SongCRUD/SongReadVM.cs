using FinalProject.Models;

namespace FinalProject.Areas.AdminArea.ViewModels.SongCRUD
{
    public class SongReadVM:BaseVM
    {
        public List<Song> Songs { get; set; } = null!;
    }
}
