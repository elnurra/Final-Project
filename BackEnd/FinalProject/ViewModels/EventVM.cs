using FinalProject.Models;

namespace FinalProject.ViewModels
{
    public class EventVM
    {
       public List<Event> Events { get; set; } = null!;
       public List<Genre> Genres { get; set; } = null!;
    }
}
