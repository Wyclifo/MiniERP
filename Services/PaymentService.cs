using MiniERP.Data;
using MiniERP.DTOs;
using MiniERP.Models;
using Microsoft.EntityFrameworkCore;
using MiniERP.Interfaces;
namespace MiniERP.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly AppDbContext _context;

        public PaymentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Payment>> GetAllAsync()
        {
            return await _context.Payments
                .Include(x => x.Customer)
                .Include(x => x.Invoice)
                .AsNoTracking()
                .OrderByDescending(x => x.PaymentDate)
                .ToListAsync();
        }

        public async Task<Payment> CreateAsync(
            CreatePaymentDto dto)
        {
            var invoice = await _context.Invoices
                .Include(x => x.Customer)
                .FirstOrDefaultAsync(x => x.Id == dto.InvoiceId);

            if (invoice == null)
                throw new InvalidOperationException(
                    "Invoice not found.");

            if (dto.Amount <= 0)
                throw new InvalidOperationException(
                    "Payment amount must be greater than zero.");

            var balance = invoice.Amount - invoice.AmountPaid;

            if (dto.Amount > balance)
                throw new InvalidOperationException(
                    "Payment cannot exceed invoice balance.");

            var payment = new Payment
            {
                PaymentNumber =
                    $"PAY-{DateTime.UtcNow:yyyyMMddHHmmssfff}",

                InvoiceId = invoice.Id,

                CustomerId = invoice.CustomerId,

                Amount = dto.Amount,

                PaymentMethod = dto.PaymentMethod,

                Reference = dto.Reference,

                PaymentDate = DateTime.UtcNow
            };

            invoice.AmountPaid += dto.Amount;

            if (invoice.AmountPaid == invoice.Amount)
                invoice.Status = "Paid";
            else
                invoice.Status = "Partially Paid";

            _context.Payments.Add(payment);

            // Bank/Cash - Debit
            _context.JournalEntries.Add(
                new JournalEntry
                {
                    Reference = payment.PaymentNumber,
                    AccountCode = "1000",
                    AccountName = "Bank/Cash",
                    Debit = payment.Amount,
                    Credit = 0,
                    Description = "Customer payment",
                    TransactionDate = payment.PaymentDate
                });

            // Accounts Receivable - Credit
            _context.JournalEntries.Add(
                new JournalEntry
                {
                    Reference = payment.PaymentNumber,
                    AccountCode = "1100",
                    AccountName = "Accounts Receivable",
                    Debit = 0,
                    Credit = payment.Amount,
                    Description = "Settlement of customer receivable",
                    TransactionDate = payment.PaymentDate
                });

            await _context.SaveChangesAsync();

            return payment;
        }
    }
}
