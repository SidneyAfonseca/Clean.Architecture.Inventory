using Clean.Architecture.Inventory.Domain.Entities;
using MediatR;

namespace Clean.Architecture.Inventory.Application.Queries
{
    public class GetInventoryTransactionByIdQuery : IRequest<InventoryTransaction>
    {
        public int Id { get; set; }
    }
}
