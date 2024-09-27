namespace Clean.Architecture.Inventory.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string PartNumber { get; set; }
        public string Name { get; set; }
        public decimal AverageCost { get; set; }
        public int QuantityInStock { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<InventoryTransaction> InventoryTransactions { get; set; }
    }
}
