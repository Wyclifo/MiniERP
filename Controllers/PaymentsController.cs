using Microsoft.AspNetCore.Mvc;
using MiniERP.DTOs;
using MiniERP.Interfaces;

namespace MiniERP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _service;

        public PaymentsController(
            IPaymentService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            CreatePaymentDto dto)
        {
            try
            {
                var payment = await _service.CreateAsync(dto);

                return Ok(payment);
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
