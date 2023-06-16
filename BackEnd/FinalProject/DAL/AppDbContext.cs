using FinalProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.DAL
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
       public DbSet<Slider> Sliders { get;set; }
        public DbSet<Artist> Artists { get;set; }
        public DbSet <Album> Albums { get;set; }
        public DbSet<Song> Songs { get;set; }
        public DbSet<Genre> Genres { get; set; }  
        public DbSet<Event> Events { get; set; }
        public DbSet<Comment> Comments { get; set; } 
        public DbSet<Subscribe> Subscribes { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<UserSong> UserSongs { get; set; }

    }
}
