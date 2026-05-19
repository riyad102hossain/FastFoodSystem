using MediatR;

namespace Ordering.API.Application.Features.Orders.Commands;

public sealed record PlaceOrderCommand(Guid OrderId, int ItemId, int Quantity) : IRequest;
