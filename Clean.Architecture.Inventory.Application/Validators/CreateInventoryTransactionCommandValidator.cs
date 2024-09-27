using Clean.Architecture.Inventory.Application.Commands;
using FluentValidation;

namespace Clean.Architecture.Inventory.Application.Validators
{
    public class CreateInventoryTransactionCommandValidator : AbstractValidator<CreateInventoryTransactionCommand>
    {
        public CreateInventoryTransactionCommandValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("Invalid product ID.");

            RuleFor(x => x.Type)
                .IsInEnum().WithMessage("Invalid transaction type.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");
        }
    }
}
