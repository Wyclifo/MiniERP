using MiniERP.Data;
using MiniERP.DTOs;
using MiniERP.Models;
using Microsoft.EntityFrameworkCore;
using MiniERP.Interfaces;
namespace MiniERP.Services
{
    public class SalesOrderService : ISalesOrderService
    {
        private readonly AppDbContext _context;

        public SalesOrderService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<SalesOrder>> GetAllAsync()
        {
            return await _context.SalesOrders
                .Include(x => x.Customer)
                .Include(x => x.Items)
                .ThenInclude(x => x.Product)
                .AsNoTracking()
                .OrderByDescending(x => x.OrderDate)
                .ToListAsync();
        }

        public async Task<SalesOrder?> GetByIdAsync(int id)
        {
            return await _context.SalesOrders
                .Include(x => x.Customer)
                .Include(x => x.Items)
                .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<SalesOrder> CreateAsync(
            CreateSalesOrderDto dto)
        {
            var customer = await _context.Customers
                .FindAsync(dto.CustomerId);

            if (customer == null)
                throw new InvalidOperationException(
                    "Customer not found.");

            if (!dto.Items.Any())
                throw new InvalidOperationException(
                    "Sales order must contain at least one item.");

            var order = new SalesOrder
            {
                OrderNumber =
                    $"SO-{DateTime.UtcNow:yyyyMMddHHmmssfff}",
                CustomerId = dto.CustomerId,
                OrderDate = DateTime.UtcNow,
                Status = "Confirmed"
            };

            decimal total = 0;

            foreach (var itemDto in dto.Items)
            {
                var product = await _context.Products
                    .FindAsync(itemDto.ProductId);

                if (product == null)
                    throw new InvalidOperationException(
                        $"Product {itemDto.ProductId} not found.");

                if (!product.IsActive)
                    throw new InvalidOperationException(
                        $"Product {product.Name} is inactive.");

                if (itemDto.Quantity <= 0)
                    throw new InvalidOperationException(
                        "Quantity must be greater than zero.");

                if (product.QuantityInStock < itemDto.Quantity)
                    throw new InvalidOperationException(
                        $"Insufficient stock for {product.Name}.");

                var item = new SalesOrderItem
                {
                    ProductId = product.Id,
                    Quantity = itemDto.Quantity,
                    UnitPrice = product.Price
                };

                order.Items.Add(item);

                total += item.Quantity * item.UnitPrice;

                product.QuantityInStock -= item.Quantity;
            }

            order.TotalAmount = total;

            _context.SalesOrders.Add(order);

            await _context.SaveChangesAsync();

            return order;
        }
    }
}
