using Microsoft.EntityFrameworkCore;
using OrderProcessing.Application;
using OrderProcessing.Domain.Entities;

namespace OrderProcessing.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Konfiguracija za Concurrency
        builder.Entity<Product>().Property(p => p.RowVersion).IsRowVersion();

        // SEEDING - Da imas podatke odmah
        builder.Entity<Product>().HasData(
            new { Id = 1, Name = "Senior .NET Course", StockQuantity = 100, Price = 199.99m, RowVersion = new byte[] { 1 } },
            new { Id = 2, Name = "Limited Edition Laptop", StockQuantity = 5, Price = 2500.00m, RowVersion = new byte[] { 1 } } // Samo 5 komada!
        );
    }
}