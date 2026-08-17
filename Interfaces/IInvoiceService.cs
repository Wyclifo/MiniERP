using MiniERP.DTOs;
using MiniERP.Models;

namespace MiniERP.Interfaces
{
    public interface IInvoiceService
    {
        Task<Invoice> CreateAsync(CreateInvoiceDto dto);

        Task<List<Invoice>> GetAllAsync();

        Task<Invoice?> GetByIdAsync(int id);
    }
}
