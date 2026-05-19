using Catalog.API.Application.Interfaces;
using Catalog.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Infrastructure.Persistence;

public sealed class CatalogDbContext : DbContext, ICatalogDbContext
{
    public CatalogDbContext(DbContextOptions<CatalogDbContext> options)
        : base(options)
    {
    }

    public DbSet<MenuItem> MenuItems { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new MenuItemEntityTypeConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}
