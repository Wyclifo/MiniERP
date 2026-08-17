using Microsoft.AspNetCore.Mvc;
using MiniERP.Data;
using MiniERP.DTOs;
using MiniERP.Models;
using Microsoft.EntityFrameworkCore;
namespace MiniERP.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CustomersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            return await _context.Customers
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await _context.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (customer == null)
                return NotFound();

            return customer;
        }

        [HttpPost]
        public async Task<ActionResult<Customer>> CreateCustomer(
            CreateCustomerDto dto)
        {
            var customer = new Customer
            {
                CustomerCode = await GenerateCustomerCode(),
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                Address = dto.Address,
                CreditLimit = dto.CreditLimit
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetCustomer),
                new { id = customer.Id },
                customer);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCustomer(
            int id,
            UpdateCustomerDto dto)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
                return NotFound();

            customer.Name = dto.Name;
            customer.Email = dto.Email;
            customer.Phone = dto.Phone;
            customer.Address = dto.Address;
            customer.CreditLimit = dto.CreditLimit;
            customer.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
                return NotFound();

            customer.IsActive = false;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<string> GenerateCustomerCode()
        {
            var count = await _context.Customers.CountAsync();

            return $"CUS-{count + 1:000}";
        }
    }
}
