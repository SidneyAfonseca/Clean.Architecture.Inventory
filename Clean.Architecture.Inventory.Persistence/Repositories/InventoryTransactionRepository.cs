using Clean.Architecture.Inventory.Application.Interfaces;
using Clean.Architecture.Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Clean.Architecture.Inventory.Persistence.Repositories
{
    public class InventoryTransactionRepository : IInventoryTransactionRepository
    {
        private readonly InventoryControlDbContext _context;

        public InventoryTransactionRepository(InventoryControlDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(InventoryTransaction transaction)
        {
            await _context.InventoryTransactions.AddAsync(transaction);
        }

        public async Task<IEnumerable<InventoryTransaction>> GetTransactionsByDateAsync(DateTime date)
        {
            return await _context.InventoryTransactions
                .Include(t => t.Product)
                .Where(t => t.TransactionDate.Date == date.Date)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
