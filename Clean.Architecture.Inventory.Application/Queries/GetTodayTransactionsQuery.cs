using Clean.Architecture.Inventory.Domain.Entities;
using MediatR;

namespace Clean.Architecture.Inventory.Application.Queries
{
    public class GetTodayTransactionsQuery : IRequest<IEnumerable<InventoryTransaction>>
    {
        public DateTime Date { get; set; }
    }
}
