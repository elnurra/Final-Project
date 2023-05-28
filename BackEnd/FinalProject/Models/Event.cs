using FinalProject.Models.BaseClass;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Models
{
    public class Event:BaseEntity
    {
        public string Title { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public string AddressUrl { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Address { get; set; } = null!;
        public DateTime CreatedTime { get; set; }
        public Artist Artist { get; set; } = null!;
        [Column(TypeName = "decimal(18,0)")]
        public decimal Price { get; set; } 
    }
}
