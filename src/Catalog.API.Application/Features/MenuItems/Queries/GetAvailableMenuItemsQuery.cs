using MediatR;

namespace Catalog.API.Application.Features.MenuItems.Queries;

public sealed record GetAvailableMenuItemsQuery : IRequest<List<MenuItemDto>>;
