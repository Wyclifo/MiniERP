using Microsoft.EntityFrameworkCore;
using MiniERP.Models;

namespace MiniERP.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(
            DbContextOptions<AppDbContext> options)
            : base(options)
        {
          
           
        }
        public DbSet<Customer> Customers => Set<Customer>();

        public DbSet<Product> Products => Set<Product>();

        public DbSet<Supplier> Suppliers => Set<Supplier>();

        public DbSet<SalesOrder> SalesOrders => Set<SalesOrder>();

        public DbSet<SalesOrderItem> SalesOrderItems => Set<SalesOrderItem>();

        public DbSet<Invoice> Invoices => Set<Invoice>();

        public DbSet<Payment> Payments => Set<Payment>();

        public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();

        public DbSet<PurchaseOrderItem> PurchaseOrderItems => Set<PurchaseOrderItem>();

        public DbSet<JournalEntry> JournalEntries => Set<JournalEntry>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Customer)
                .WithMany(c => c.Invoices)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.SalesOrder)
                .WithOne(s => s.Invoice)
                .OnDelete(DeleteBehavior.Restrict);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties()
                             .Where(p => p.ClrType == typeof(decimal)))
                {
                    property.SetPrecision(18);
                    property.SetScale(2);
                }
            }
        }

    }
}
