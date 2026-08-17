using MiniERP.Data;
using MiniERP.DTOs;
using MiniERP.Models;
using Microsoft.EntityFrameworkCore;
using MiniERP.Interfaces;
namespace MiniERP.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly AppDbContext _context;

        public InvoiceService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Invoice>> GetAllAsync()
        {
            return await _context.Invoices
                .Include(x => x.Customer)
                .Include(x => x.SalesOrder)
                .AsNoTracking()
                .OrderByDescending(x => x.InvoiceDate)
                .ToListAsync();
        }

        public async Task<Invoice?> GetByIdAsync(int id)
        {
            return await _context.Invoices
                .Include(x => x.Customer)
                .Include(x => x.SalesOrder)
                .Include(x => x.Payments)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Invoice> CreateAsync(
            CreateInvoiceDto dto)
        {
            var salesOrder = await _context.SalesOrders
                .Include(x => x.Customer)
                .Include(x => x.Invoice)
                .FirstOrDefaultAsync(x => x.Id == dto.SalesOrderId);

            if (salesOrder == null)
                throw new InvalidOperationException(
                    "Sales order not found.");

            if (salesOrder.Invoice != null)
                throw new InvalidOperationException(
                    "An invoice already exists for this sales order.");

            var invoice = new Invoice
            {
                InvoiceNumber =
                    $"INV-{DateTime.UtcNow:yyyyMMddHHmmssfff}",

                SalesOrderId = salesOrder.Id,

                CustomerId = salesOrder.CustomerId,

                InvoiceDate = DateTime.UtcNow,

                DueDate = DateTime.UtcNow.AddDays(
                    dto.PaymentTermDays),

                Amount = salesOrder.TotalAmount,

                AmountPaid = 0,

                Status = "Unpaid"
            };

            _context.Invoices.Add(invoice);

            // Accounts Receivable - Debit
            _context.JournalEntries.Add(
                new JournalEntry
                {
                    Reference = invoice.InvoiceNumber,
                    AccountCode = "1100",
                    AccountName = "Accounts Receivable",
                    Debit = invoice.Amount,
                    Credit = 0,
                    Description = "Customer invoice",
                    TransactionDate = invoice.InvoiceDate
                });

            // Sales Revenue - Credit
            _context.JournalEntries.Add(
                new JournalEntry
                {
                    Reference = invoice.InvoiceNumber,
                    AccountCode = "4000",
                    AccountName = "Sales Revenue",
                    Debit = 0,
                    Credit = invoice.Amount,
                    Description = "Sales revenue",
                    TransactionDate = invoice.InvoiceDate
                });

            await _context.SaveChangesAsync();

            return invoice;
        }
    }
}
