using Catalog.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Application.Interfaces;

public interface ICatalogDbContext
{
    DbSet<MenuItem> MenuItems { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
