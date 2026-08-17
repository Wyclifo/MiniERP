using Microsoft.AspNetCore.Mvc;
using MiniERP.Data;
using MiniERP.DTOs;
using MiniERP.Models;
using Microsoft.EntityFrameworkCore;
namespace MiniERP.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class PurchaseOrdersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PurchaseOrdersController(
            AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _context.PurchaseOrders
                .Include(x => x.Supplier)
                .Include(x => x.Items)
                .ThenInclude(x => x.Product)
                .AsNoTracking()
                .OrderByDescending(x => x.OrderDate)
                .ToListAsync();

            return Ok(orders);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _context.PurchaseOrders
                .Include(x => x.Supplier)
                .Include(x => x.Items)
                .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            CreatePurchaseOrderDto dto)
        {
            var supplier = await _context.Suppliers
                .FindAsync(dto.SupplierId);

            if (supplier == null)
                return BadRequest("Supplier not found.");

            if (!dto.Items.Any())
                return BadRequest(
                    "Purchase order must contain items.");

            var order = new PurchaseOrder
            {
                OrderNumber =
                    $"PO-{DateTime.UtcNow:yyyyMMddHHmmssfff}",

                SupplierId = dto.SupplierId,

                OrderDate = DateTime.UtcNow,

                Status = "Received"
            };

            decimal total = 0;

            foreach (var itemDto in dto.Items)
            {
                var product = await _context.Products
                    .FindAsync(itemDto.ProductId);

                if (product == null)
                    return BadRequest(
                        $"Product {itemDto.ProductId} not found.");

                if (itemDto.Quantity <= 0)
                    return BadRequest(
                        "Quantity must be greater than zero.");

                if (itemDto.UnitCost < 0)
                    return BadRequest(
                        "Unit cost cannot be negative.");

                var item = new PurchaseOrderItem
                {
                    ProductId = product.Id,
                    Quantity = itemDto.Quantity,
                    UnitCost = itemDto.UnitCost
                };

                order.Items.Add(item);

                total += item.Quantity * item.UnitCost;

                // Goods received
                product.QuantityInStock += item.Quantity;
            }

            order.TotalAmount = total;

            _context.PurchaseOrders.Add(order);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetById),
                new { id = order.Id },
                order);
        }
    }
}
