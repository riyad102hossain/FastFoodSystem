using Catalog.API.Application.Caching;
using Catalog.API.Application.Interfaces;
using Catalog.API.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace Catalog.API.Application.Features.MenuItems.Commands;

public sealed class CreateMenuItemCommandHandler : IRequestHandler<CreateMenuItemCommand, int>
{
    private readonly ICatalogDbContext _dbContext;
    private readonly IDistributedCache _cache;

    public CreateMenuItemCommandHandler(ICatalogDbContext dbContext, IDistributedCache cache)
    {
        _dbContext = dbContext;
        _cache = cache;
    }

    public async Task<int> Handle(CreateMenuItemCommand request, CancellationToken cancellationToken)
    {
        var menuItem = MenuItem.Create(
            request.Name,
            request.Description,
            request.Price,
            request.IsAvailable,
            request.InventoryCount);

        _dbContext.MenuItems.Add(menuItem);
        await _dbContext.SaveChangesAsync(cancellationToken);
        await _cache.RemoveAsync(CacheKeys.AvailableMenuItems, cancellationToken);

        return menuItem.Id;
    }
}
