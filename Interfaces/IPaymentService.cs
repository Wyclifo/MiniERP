using MiniERP.DTOs;
using MiniERP.Models;

namespace MiniERP.Interfaces
{
    public interface IPaymentService
    {
        Task<Payment> CreateAsync(CreatePaymentDto dto);

        Task<List<Payment>> GetAllAsync();
    }
}
