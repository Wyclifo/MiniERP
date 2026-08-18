using Microsoft.AspNetCore.Mvc;
using MiniERP.Data;
using MiniERP.DTOs;
using MiniERP.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MiniERP.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(AppDbContext context, ILogger<CustomersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            try
            {
                return await _context.Customers
                    .AsNoTracking()
                    .OrderBy(x => x.Name)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                 _logger?.LogError(ex, "Error retrieving customers.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving customers.");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            try
            {
                var customer = await _context.Customers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id);
                if (customer == null)
                    return NotFound();
                return customer;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Error retrieving customer with ID {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the customer.");
            }
           
        }

        [HttpPost]
        public async Task<ActionResult<Customer>> CreateCustomer(
            CreateCustomerDto dto)
        {
            try
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
            } catch (Exception ex) {
                _logger?.LogError(ex, "Error creating customer.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the customer.");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCustomer(
            int id,
            UpdateCustomerDto dto)
        {
            try
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
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Error updating customer with ID {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the customer.");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            try
            {
                var customer = await _context.Customers.FindAsync(id);

                if (customer == null)
                    return NotFound();

                customer.IsActive = false;

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Error deleting customer with ID {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the customer.");
            }
        }

        private async Task<string> GenerateCustomerCode()
        {

            try
            {
                var count = await _context.Customers.CountAsync();

                return $"CUS-{count + 1:000}";
            } catch (Exception ex) {
                _logger?.LogError(ex, "Error generating customer code.");
                throw;
            }
        }
    }
}
