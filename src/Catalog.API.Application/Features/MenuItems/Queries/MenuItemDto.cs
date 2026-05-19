namespace Catalog.API.Application.Features.MenuItems.Queries;

public sealed record MenuItemDto(
    int Id,
    string Name,
    string Description,
    decimal Price,
    bool IsAvailable,
    int InventoryCount,
    DateTime CreatedAt);
