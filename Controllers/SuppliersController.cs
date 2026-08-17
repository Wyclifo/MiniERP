using Microsoft.AspNetCore.Mvc;
using MiniERP.Data;
using MiniERP.DTOs;
using MiniERP.Models;
using Microsoft.EntityFrameworkCore;
namespace MiniERP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SuppliersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SuppliersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Supplier>>> GetSuppliers()
        {
            return await _context.Suppliers
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Supplier>> GetSupplier(int id)
        {
            var supplier = await _context.Suppliers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (supplier == null)
                return NotFound();

            return supplier;
        }

        [HttpPost]
        public async Task<ActionResult<Supplier>> CreateSupplier(
            CreateSupplierDto dto)
        {
            var supplier = new Supplier
            {
                SupplierCode =
                    $"SUP-{await _context.Suppliers.CountAsync() + 1:000}",
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                Address = dto.Address
            };

            _context.Suppliers.Add(supplier);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetSupplier),
                new { id = supplier.Id },
                supplier);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateSupplier(
            int id,
            UpdateSupplierDto dto)
        {
            var supplier = await _context.Suppliers.FindAsync(id);

            if (supplier == null)
                return NotFound();

            supplier.Name = dto.Name;
            supplier.Email = dto.Email;
            supplier.Phone = dto.Phone;
            supplier.Address = dto.Address;
            supplier.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);

            if (supplier == null)
                return NotFound();

            supplier.IsActive = false;

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
