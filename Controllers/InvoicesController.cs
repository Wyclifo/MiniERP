using Microsoft.AspNetCore.Mvc;
using MiniERP.DTOs;
using MiniERP.Interfaces;

namespace MiniERP.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceService _service;

        public InvoicesController(
            IInvoiceService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var invoice = await _service.GetByIdAsync(id);

            if (invoice == null)
                return NotFound();

            return Ok(invoice);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            CreateInvoiceDto dto)
        {
            try
            {
                var invoice = await _service.CreateAsync(dto);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = invoice.Id },
                    invoice);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }
    }
}
