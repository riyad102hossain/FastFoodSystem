using System.Text.Json;
using Catalog.API.Application.Caching;
using Catalog.API.Application.Features.MenuItems.Queries;
using Catalog.API.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace Catalog.API.Application.Features.MenuItems.Queries;

public sealed class GetAvailableMenuItemsQueryHandler : IRequestHandler<GetAvailableMenuItemsQuery, List<MenuItemDto>>
{
    private static readonly JsonSerializerOptions SerializerOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    private readonly ICatalogDbContext _dbContext;
    private readonly IDistributedCache _cache;

    public GetAvailableMenuItemsQueryHandler(ICatalogDbContext dbContext, IDistributedCache cache)
    {
        _dbContext = dbContext;
        _cache = cache;
    }

    public async Task<List<MenuItemDto>> Handle(GetAvailableMenuItemsQuery request, CancellationToken cancellationToken)
    {
        var cachedResponse = await _cache.GetStringAsync(CacheKeys.AvailableMenuItems, cancellationToken);
        if (cachedResponse is not null)
        {
            return JsonSerializer.Deserialize<List<MenuItemDto>>(cachedResponse, SerializerOptions)
                ?? new List<MenuItemDto>();
        }

        var menuItems = await _dbContext.MenuItems
            .AsNoTracking()
            .Where(x => x.IsAvailable)
            .OrderBy(x => x.Name)
            .Select(x => new MenuItemDto(
                x.Id,
                x.Name,
                x.Description,
                x.Price,
                x.IsAvailable,
                x.InventoryCount,
                x.CreatedAt))
            .ToListAsync(cancellationToken);

        var serialized = JsonSerializer.Serialize(menuItems, SerializerOptions);
        await _cache.SetStringAsync(
            CacheKeys.AvailableMenuItems,
            serialized,
            new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2) },
            cancellationToken);

        return menuItems;
    }
}
