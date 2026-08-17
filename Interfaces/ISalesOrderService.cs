using MiniERP.DTOs;
using MiniERP.Models;

namespace MiniERP.Interfaces
{
    public interface ISalesOrderService
    {
        Task<SalesOrder> CreateAsync(CreateSalesOrderDto dto);

        Task<SalesOrder?> GetByIdAsync(int id);

        Task<List<SalesOrder>> GetAllAsync();
    }
}
