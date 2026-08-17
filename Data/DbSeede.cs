using Microsoft.EntityFrameworkCore;
using MiniERP.Models;

namespace MiniERP.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(
            AppDbContext context)
        {
            await context.Database.MigrateAsync();

            if (!await context.Customers.AnyAsync())
            {
                context.Customers.AddRange(
                    new Customer
                    {
                        CustomerCode = "CUS-001",
                        Name = "ABC Enterprises",
                        Email = "info@abc.com",
                        Phone = "0712345678",
                        Address = "Nairobi",
                        CreditLimit = 100000
                    },
                    new Customer
                    {
                        CustomerCode = "CUS-002",
                        Name = "XYZ Limited",
                        Email = "info@xyz.com",
                        Phone = "0722345678",
                        Address = "Mombasa",
                        CreditLimit = 200000
                    }
                );
            }

            if (!await context.Suppliers.AnyAsync())
            {
                context.Suppliers.AddRange(
                    new Supplier
                    {
                        SupplierCode = "SUP-001",
                        Name = "Tech Supplies Ltd",
                        Email = "sales@techsupplies.com",
                        Phone = "0733123456",
                        Address = "Nairobi"
                    },
                    new Supplier
                    {
                        SupplierCode = "SUP-002",
                        Name = "General Suppliers Ltd",
                        Email = "info@generalsuppliers.com",
                        Phone = "0744123456",
                        Address = "Kisumu"
                    }
                );
            }

            if (!await context.Products.AnyAsync())
            {
                context.Products.AddRange(
                    new Product
                    {
                        ProductCode = "PRD-001",
                        Name = "Laptop",
                        Description = "Business laptop",
                        Price = 85000,
                        CostPrice = 65000,
                        QuantityInStock = 20,
                        ReorderLevel = 5
                    },
                    new Product
                    {
                        ProductCode = "PRD-002",
                        Name = "Wireless Mouse",
                        Description = "Wireless optical mouse",
                        Price = 2500,
                        CostPrice = 1500,
                        QuantityInStock = 100,
                        ReorderLevel = 20
                    },
                    new Product
                    {
                        ProductCode = "PRD-003",
                        Name = "Keyboard",
                        Description = "USB keyboard",
                        Price = 3000,
                        CostPrice = 1800,
                        QuantityInStock = 75,
                        ReorderLevel = 15
                    }
                );
            }

            await context.SaveChangesAsync();
        }
    }
}
