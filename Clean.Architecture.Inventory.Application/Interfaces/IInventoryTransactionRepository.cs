using Clean.Architecture.Inventory.Domain.Entities;

namespace Clean.Architecture.Inventory.Application.Interfaces
{
    public interface IInventoryTransactionRepository
    {
        Task AddAsync(InventoryTransaction transaction);
        Task<IEnumerable<InventoryTransaction>> GetTransactionsByDateAsync(DateTime date);
        Task SaveChangesAsync();
    }
}
