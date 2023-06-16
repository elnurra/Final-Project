using FinalProject.Models;

namespace FinalProject.Areas.AdminArea.ViewModels.EventCRUD
{
    public class EventReadVM:BaseVM
    {
        public List<Event> Events { get; set; } = null!;
    }
}
