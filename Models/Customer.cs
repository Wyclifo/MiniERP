namespace MiniERP.Models
{
    public class Customer
    {
        public int Id { get; set; }

        public string CustomerCode { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public decimal CreditLimit { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<SalesOrder> SalesOrders { get; set; }
            = new List<SalesOrder>();

        public ICollection<Invoice> Invoices { get; set; }
            = new List<Invoice>();

        public ICollection<Payment> Payments { get; set; }
            = new List<Payment>();
    }
}
