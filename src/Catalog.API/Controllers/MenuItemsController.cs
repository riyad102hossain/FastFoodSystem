using Catalog.API.Application.Features.MenuItems.Commands;
using Catalog.API.Application.Features.MenuItems.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers;

[ApiController]
[Route("api/menu-items")]
public sealed class MenuItemsController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateMenuItem([FromBody] CreateMenuItemCommand command, CancellationToken cancellationToken)
    {
        var id = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetAvailableMenuItems), null, new { id });
    }

    [HttpGet("available")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAvailableMenuItems(CancellationToken cancellationToken)
    {
        var menuItems = await mediator.Send(new GetAvailableMenuItemsQuery(), cancellationToken);
        return Ok(menuItems);
    }
}
