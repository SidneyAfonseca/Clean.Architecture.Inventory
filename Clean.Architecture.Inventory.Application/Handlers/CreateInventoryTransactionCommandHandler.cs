using Clean.Architecture.Inventory.Application.Commands;
using Clean.Architecture.Inventory.Application.Interfaces;
using Clean.Architecture.Inventory.Domain.Entities;
using MediatR;

namespace Clean.Architecture.Inventory.Application.Handlers
{
    public class CreateInventoryTransactionCommandHandler : IRequestHandler<CreateInventoryTransactionCommand, int>
    {
        private readonly IProductRepository _productRepository;
        private readonly IInventoryTransactionRepository _transactionRepository;

        public CreateInventoryTransactionCommandHandler(IProductRepository productRepository, IInventoryTransactionRepository transactionRepository)
        {
            _productRepository = productRepository;
            _transactionRepository = transactionRepository;
        }

        public async Task<int> Handle(CreateInventoryTransactionCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.ProductId);
            if (product == null)
                throw new Exception($"Product with ID {request.ProductId} does not exist.");

            if (request.Type == TransactionType.Exit)
            {
                if (product.QuantityInStock < request.Quantity)
                    throw new Exception("Saldo de estoque indisponível.");

                product.QuantityInStock -= request.Quantity;
            }
            else if (request.Type == TransactionType.Entry)
            {
                product.QuantityInStock += request.Quantity;
            }

            product.UpdatedAt = DateTime.UtcNow;
            _productRepository.Update(product);
            await _productRepository.SaveChangesAsync();

            var transaction = new InventoryTransaction
            {
                ProductId = request.ProductId,
                Type = request.Type,
                Quantity = request.Quantity,
                Cost = product.AverageCost * request.Quantity,
                TransactionDate = DateTime.UtcNow
            };

            await _transactionRepository.AddAsync(transaction);
            await _transactionRepository.SaveChangesAsync();

            return transaction.Id;
        }
    }
}
