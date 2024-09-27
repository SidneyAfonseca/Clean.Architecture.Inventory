using MediatR;

namespace Clean.Architecture.Inventory.Application.Commands
{
    public class UpdateProductCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public string PartNumber { get; set; }
        public string Name { get; set; }
        public decimal AverageCost { get; set; }
        public int QuantityInStock { get; set; }
    }
}
