using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.API.Application.Features.Orders.Commands;

namespace Ordering.API.Controllers;

[ApiController]
[Route("api/orders")]
public sealed class OrdersController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderCommand command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return Accepted(new { command.OrderId });
    }
}
