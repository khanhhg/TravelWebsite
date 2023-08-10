using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using Travels.Models.EF;

namespace Travels.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Blog> Blog { get; set; }
        public DbSet<Contact> Contact { get; set; }
        public DbSet<MenuPage> MenuPage { get; set; }
        public DbSet<Place> Place { get; set; }
        public DbSet<Tour> Tour { get; set; }
        public DbSet<TourDetails> TourDetails { get; set; }
    }
}