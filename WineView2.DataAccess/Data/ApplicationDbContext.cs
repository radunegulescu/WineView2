using Microsoft.EntityFrameworkCore;
using WineView2.Models;

namespace WineView2.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Color> Colors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Color>().HasData(
                new Color { Id = 1, Name = "Red"},
                new Color { Id = 2, Name = "Rose"},
                new Color { Id = 3, Name = "White"}
                );
        }
    }
}
