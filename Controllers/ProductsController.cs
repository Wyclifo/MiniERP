using Microsoft.AspNetCore.Mvc;
using MiniERP.Data;
using MiniERP.DTOs;
using MiniERP.Models;
using Microsoft.EntityFrameworkCore;
namespace MiniERP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (product == null)
                return NotFound();

            return product;
        }

        [HttpGet("low-stock")]
        public async Task<ActionResult<IEnumerable<Product>>> GetLowStock()
        {
            var products = await _context.Products
                .Where(x => x.QuantityInStock <= x.ReorderLevel)
                .OrderBy(x => x.QuantityInStock)
                .ToListAsync();

            return products;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(
            CreateProductDto dto)
        {
            if (await _context.Products.AnyAsync(
                    x => x.ProductCode == dto.ProductCode))
            {
                return Conflict("Product code already exists.");
            }

            var product = new Product
            {
                ProductCode = dto.ProductCode,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                CostPrice = dto.CostPrice,
                QuantityInStock = dto.QuantityInStock,
                ReorderLevel = dto.ReorderLevel
            };

            _context.Products.Add(product);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetProduct),
                new { id = product.Id },
                product);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateProduct(
            int id,
            UpdateProductDto dto)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound();

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.CostPrice = dto.CostPrice;
            product.QuantityInStock = dto.QuantityInStock;
            product.ReorderLevel = dto.ReorderLevel;
            product.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound();

            product.IsActive = false;

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
