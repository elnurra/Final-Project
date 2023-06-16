using FinalProject.Models;

namespace FinalProject.Areas.AdminArea.ViewModels.GenreCRUD
{
    public class GenreReadVM:BaseVM
    {
        public List<Genre> Genres { get; set; } = null!;

    }
}
