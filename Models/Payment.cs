namespace MiniERP.Models
{
    public class Payment
    {
        public int Id { get; set; }

        public string PaymentNumber { get; set; } = string.Empty;

        public int InvoiceId { get; set; }

        public Invoice Invoice { get; set; } = null!;

        public int CustomerId { get; set; }

        public Customer Customer { get; set; } = null!;

        public decimal Amount { get; set; }

        public string PaymentMethod { get; set; } = "Cash";

        public string Reference { get; set; } = string.Empty;

        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
    }
}
