using Clean.Architecture.Inventory.Application.Interfaces;
using Clean.Architecture.Inventory.Application.Queries;
using Clean.Architecture.Inventory.Domain.Entities;
using MediatR;

namespace Clean.Architecture.Inventory.Application.Handlers
{
    public class GetTodayTransactionsQueryHandler : IRequestHandler<GetTodayTransactionsQuery, IEnumerable<InventoryTransaction>>
    {
        private readonly IInventoryTransactionRepository _transactionRepository;

        public GetTodayTransactionsQueryHandler(IInventoryTransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<IEnumerable<InventoryTransaction>> Handle(GetTodayTransactionsQuery request, CancellationToken cancellationToken)
        {
            return await _transactionRepository.GetTransactionsByDateAsync(request.Date);
        }
    }
}
