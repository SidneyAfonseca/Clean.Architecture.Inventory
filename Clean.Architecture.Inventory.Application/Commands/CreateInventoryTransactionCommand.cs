using Clean.Architecture.Inventory.Domain.Entities;
using MediatR;

namespace Clean.Architecture.Inventory.Application.Commands
{
    public class CreateInventoryTransactionCommand : IRequest<int>
    {
        public int ProductId { get; set; }
        public TransactionType Type { get; set; } // Entry ou Exit
        public int Quantity { get; set; }
    }
}
