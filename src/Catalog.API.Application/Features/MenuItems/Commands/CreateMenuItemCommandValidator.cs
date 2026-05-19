using FluentValidation;

namespace Catalog.API.Application.Features.MenuItems.Commands;

public sealed class CreateMenuItemCommandValidator : AbstractValidator<CreateMenuItemCommand>
{
    public CreateMenuItemCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name cannot be empty.");

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("Price must be greater than 0.");
    }
}
