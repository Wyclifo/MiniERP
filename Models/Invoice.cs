namespace MiniERP.Models
{
    public class Invoice
    {
        public int Id { get; set; }

        public string InvoiceNumber { get; set; } = string.Empty;

        public int SalesOrderId { get; set; }

        public SalesOrder SalesOrder { get; set; } = null!;

        public int CustomerId { get; set; }

        public Customer Customer { get; set; } = null!;

        public DateTime InvoiceDate { get; set; } = DateTime.UtcNow;

        public DateTime DueDate { get; set; }

        public decimal Amount { get; set; }

        public decimal AmountPaid { get; set; }

        public string Status { get; set; } = "Unpaid";

        public decimal Balance =>
            Amount - AmountPaid;

        public ICollection<Payment> Payments { get; set; }
            = new List<Payment>();
    }
}
