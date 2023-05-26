using FinalProject.Models.BaseClass;

namespace FinalProject.Models
{
    public class Artist:BaseEntity
    {
        public string Name { get; set; } = null!;
        public List<Album> Albums { get; set; } = null!;
        

        


    }
}
