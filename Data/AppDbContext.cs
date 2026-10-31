using Microsoft.EntityFrameworkCore;
using wt_lab7_7postgres_efcore_strelchonok.Models;

namespace wt_lab7_7postgres_efcore_strelchonok.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Book> Books => Set<Book>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>().HasData(
            new Book
            {
                Id = 1,
                Title = "Война и мир",
                Author = "Л.Н. Толстой",
                Year = 1869,
                Price = 45.00m,
                IsAvailable = true,
                CreatedAt = new DateTime(2026, 1, 1) 
            },
            new Book
            {
                Id = 2,
                Title = "Мастер и Маргарита",
                Author = "М.А. Булгаков",
                Year = 1967,
                Price = 30.00m,
                IsAvailable = false,
                CreatedAt = new DateTime(2026, 1, 2)
            }
        );
    }
}

