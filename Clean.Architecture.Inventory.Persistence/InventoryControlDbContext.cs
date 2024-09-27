using Clean.Architecture.Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Clean.Architecture.Inventory.Persistence
{
    public class InventoryControlDbContext : DbContext
    {
        public InventoryControlDbContext(DbContextOptions<InventoryControlDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<InventoryTransaction> InventoryTransactions { get; set; }
        public DbSet<ErrorLog> ErrorLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurações adicionais podem ser feitas aqui

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.PartNumber).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.AverageCost).HasColumnType("decimal(18,2)");
                entity.Property(e => e.QuantityInStock).IsRequired();
                entity.HasMany(e => e.InventoryTransactions)
                      .WithOne(t => t.Product)
                      .HasForeignKey(t => t.ProductId);
            });

            modelBuilder.Entity<InventoryTransaction>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Type).IsRequired();
                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.Cost).HasColumnType("decimal(18,2)");
                entity.Property(e => e.TransactionDate).IsRequired();
            });

            modelBuilder.Entity<ErrorLog>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Message).IsRequired();
                entity.Property(e => e.StackTrace).IsRequired(false);
                entity.Property(e => e.OccurredAt).IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
