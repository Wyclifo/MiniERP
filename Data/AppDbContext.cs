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

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>()
                .HasIndex(x => x.CustomerCode)
                .IsUnique();

            modelBuilder.Entity<Product>()
                .HasIndex(x => x.ProductCode)
                .IsUnique();

            modelBuilder.Entity<Supplier>()
                .HasIndex(x => x.SupplierCode)
                .IsUnique();

            modelBuilder.Entity<SalesOrder>()
                .HasIndex(x => x.OrderNumber)
                .IsUnique();

            modelBuilder.Entity<Invoice>()
                .HasIndex(x => x.InvoiceNumber)
                .IsUnique();

            modelBuilder.Entity<Payment>()
                .HasIndex(x => x.PaymentNumber)
                .IsUnique();

            modelBuilder.Entity<PurchaseOrder>()
                .HasIndex(x => x.OrderNumber)
                .IsUnique();

            modelBuilder.Entity<Product>()
                .Property(x => x.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Product>()
                .Property(x => x.CostPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Customer>()
                .Property(x => x.CreditLimit)
                .HasPrecision(18, 2);

            modelBuilder.Entity<SalesOrder>()
                .Property(x => x.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<SalesOrderItem>()
                .Property(x => x.UnitPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Invoice>()
                .Property(x => x.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Invoice>()
                .Property(x => x.AmountPaid)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Payment>()
                .Property(x => x.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PurchaseOrder>()
                .Property(x => x.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PurchaseOrderItem>()
                .Property(x => x.UnitCost)
                .HasPrecision(18, 2);

            modelBuilder.Entity<JournalEntry>()
                .Property(x => x.Debit)
                .HasPrecision(18, 2);

            modelBuilder.Entity<JournalEntry>()
                .Property(x => x.Credit)
                .HasPrecision(18, 2);

            modelBuilder.Entity<SalesOrder>()
                .HasOne(x => x.Customer)
                .WithMany(x => x.SalesOrders)
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SalesOrderItem>()
                .HasOne(x => x.SalesOrder)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.SalesOrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SalesOrderItem>()
                .HasOne(x => x.Product)
                .WithMany(x => x.SalesOrderItems)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Invoice>()
                .HasOne(x => x.SalesOrder)
                .WithOne(x => x.Invoice)
                .HasForeignKey<Invoice>(x => x.SalesOrderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Invoice>()
                .HasOne(x => x.Customer)
                .WithMany(x => x.Invoices)
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
                .HasOne(x => x.Invoice)
                .WithMany(x => x.Payments)
                .HasForeignKey(x => x.InvoiceId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
                .HasOne(x => x.Customer)
                .WithMany(x => x.Payments)
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PurchaseOrder>()
                .HasOne(x => x.Supplier)
                .WithMany(x => x.PurchaseOrders)
                .HasForeignKey(x => x.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PurchaseOrderItem>()
                .HasOne(x => x.PurchaseOrder)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.PurchaseOrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PurchaseOrderItem>()
                .HasOne(x => x.Product)
                .WithMany(x => x.PurchaseOrderItems)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
