using MassTransit;
using MediatR;
using Shared.Events;

namespace Ordering.API.Application.Features.Orders.Commands;

public sealed class PlaceOrderCommandHandler : IRequestHandler<PlaceOrderCommand>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public PlaceOrderCommandHandler(IPublishEndpoint publishEndpoint) => _publishEndpoint = publishEndpoint;

    public async Task<Unit> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
    {
        var orderPlaced = new OrderPlacedEvent(request.OrderId, request.ItemId, request.Quantity);
        await _publishEndpoint.Publish(orderPlaced, cancellationToken);
        return Unit.Value;
    }
}
