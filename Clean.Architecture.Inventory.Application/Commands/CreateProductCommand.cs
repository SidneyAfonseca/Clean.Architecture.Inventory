using MediatR;

namespace Clean.Architecture.Inventory.Application.Commands
{
    public class CreateProductCommand : IRequest<int>
    {
        public string PartNumber { get; set; }
        public string Name { get; set; }
        public decimal AverageCost { get; set; }
        public int QuantityInStock { get; set; }
    }
}
