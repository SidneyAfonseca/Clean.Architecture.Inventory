namespace Clean.Architecture.Inventory.Domain.Entities
{
    public class InventoryTransaction
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public TransactionType Type { get; set; }
        public int Quantity { get; set; }
        public decimal Cost { get; set; }
        public DateTime TransactionDate { get; set; }
        public Product Product { get; set; }
    }

    public enum TransactionType
    {
        Entry = 1,
        Exit = 2
    }
}
