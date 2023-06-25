using CafeLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace CafeLibrary.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }

        //Seed data to database
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    Id = 1,
                    Name = "Sci Fi",
                    DisplayOrder = 2,
                },
                new Category
                {
                    Id = 2,
                    Name = "Action",
                    DisplayOrder = 1,
                },
                new Category
                {
                    Id = 3,
                    Name = "History",
                    DisplayOrder = 4,
                }
               );
        }
    }
}
