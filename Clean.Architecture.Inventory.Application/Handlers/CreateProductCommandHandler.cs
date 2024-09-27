using Clean.Architecture.Inventory.Application.Commands;
using Clean.Architecture.Inventory.Application.Interfaces;
using Clean.Architecture.Inventory.Domain.Entities;
using MediatR;

namespace Clean.Architecture.Inventory.Application.Handlers
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly IProductRepository _productRepository;

        public CreateProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                PartNumber = request.PartNumber,
                Name = request.Name,
                AverageCost = request.AverageCost,
                QuantityInStock = request.QuantityInStock,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _productRepository.AddAsync(product);
            await _productRepository.SaveChangesAsync();

            return product.Id;
        }
    }
}
