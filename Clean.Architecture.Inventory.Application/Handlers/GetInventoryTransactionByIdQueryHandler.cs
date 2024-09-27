using Clean.Architecture.Inventory.Application.Interfaces;
using Clean.Architecture.Inventory.Application.Queries;
using Clean.Architecture.Inventory.Domain.Entities;
using MediatR;

namespace Clean.Architecture.Inventory.Application.Handlers
{
    public class GetInventoryTransactionByIdQueryHandler : IRequestHandler<GetInventoryTransactionByIdQuery, InventoryTransaction>
    {
        private readonly IInventoryTransactionRepository _transactionRepository;

        public GetInventoryTransactionByIdQueryHandler(IInventoryTransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<InventoryTransaction> Handle(GetInventoryTransactionByIdQuery request, CancellationToken cancellationToken)
        {
            // Implementação similar à obtenção por ID
            // Para simplificar, supondo que haja um método GetByIdAsync no repositório
            // Caso contrário, você pode ajustar conforme necessário
            throw new NotImplementedException("Implement GetInventoryTransactionByIdQueryHandler.");
        }
    }
}
