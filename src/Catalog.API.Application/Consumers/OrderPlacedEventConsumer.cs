using Catalog.API.Application.Caching;
using Catalog.API.Application.Interfaces;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Shared.Events;

namespace Catalog.API.Application.Consumers;

public sealed class OrderPlacedEventConsumer : IConsumer<OrderPlacedEvent>
{
    private readonly ICatalogDbContext _dbContext;
    private readonly IDistributedCache _cache;

    public OrderPlacedEventConsumer(ICatalogDbContext dbContext, IDistributedCache cache)
    {
        _dbContext = dbContext;
        _cache = cache;
    }

    public async Task Consume(ConsumeContext<OrderPlacedEvent> context)
    {
        var message = context.Message;

        var menuItem = await _dbContext.MenuItems
            .FirstOrDefaultAsync(x => x.Id == message.ItemId, context.CancellationToken);

        if (menuItem is null)
        {
            return;
        }

        var updatedInventory = Math.Max(0, menuItem.InventoryCount - message.Quantity);
        menuItem.AdjustInventory(updatedInventory);

        await _dbContext.SaveChangesAsync(context.CancellationToken);
        await _cache.RemoveAsync(CacheKeys.AvailableMenuItems, context.CancellationToken);
    }
}
