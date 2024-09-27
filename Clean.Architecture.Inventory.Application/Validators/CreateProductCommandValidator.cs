using Clean.Architecture.Inventory.Application.Commands;
using FluentValidation;

namespace Clean.Architecture.Inventory.Application.Validators
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.PartNumber)
                .NotEmpty().WithMessage("Part number is required.")
                .MaximumLength(50);

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100);

            RuleFor(x => x.AverageCost)
                .GreaterThanOrEqualTo(0).WithMessage("Average cost must be non-negative.");

            RuleFor(x => x.QuantityInStock)
                .GreaterThanOrEqualTo(0).WithMessage("Quantity in stock must be non-negative.");
        }
    }
}
