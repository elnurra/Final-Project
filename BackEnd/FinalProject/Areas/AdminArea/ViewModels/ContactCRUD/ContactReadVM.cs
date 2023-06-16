using FinalProject.Models;

namespace FinalProject.Areas.AdminArea.ViewModels.ContactCRUD
{
    public class ContactReadVM:BaseVM
    {
        public List<Contact> Contacts { get; set; } = null!;
        
    }
}
