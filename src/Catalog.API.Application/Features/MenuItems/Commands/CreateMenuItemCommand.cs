using MediatR;

namespace Catalog.API.Application.Features.MenuItems.Commands;

public sealed record CreateMenuItemCommand(
    string Name,
    string Description,
    decimal Price,
    bool IsAvailable,
    int InventoryCount) : IRequest<int>;
