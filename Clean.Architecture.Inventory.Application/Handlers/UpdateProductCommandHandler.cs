using Clean.Architecture.Inventory.Application.Commands;
using Clean.Architecture.Inventory.Application.Interfaces;
using MediatR;

namespace Clean.Architecture.Inventory.Application.Handlers
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Unit>
    {
        private readonly IProductRepository _productRepository;

        public UpdateProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.Id);
            if (product == null)
            {
                throw new Exception($"Product with ID {request.Id} not found.");
            }

            product.PartNumber = request.PartNumber;
            product.Name = request.Name;
            product.AverageCost = request.AverageCost;
            product.QuantityInStock = request.QuantityInStock;
            product.UpdatedAt = DateTime.UtcNow;

            _productRepository.Update(product);
            await _productRepository.SaveChangesAsync();

            return Unit.Value; // Retorna Unit.Value para indicar conclusão
        }
    }
}
