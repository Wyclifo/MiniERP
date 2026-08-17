namespace MiniERP.Models
{
    public class SalesOrder
    {
        public int Id { get; set; }

        public string OrderNumber { get; set; } = string.Empty;

        public int CustomerId { get; set; }

        public Customer Customer { get; set; } = null!;

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public string Status { get; set; } = "Pending";

        public decimal TotalAmount { get; set; }

        public ICollection<SalesOrderItem> Items { get; set; }
            = new List<SalesOrderItem>();

        public Invoice? Invoice { get; set; }
    }
}
