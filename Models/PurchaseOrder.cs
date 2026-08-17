namespace MiniERP.Models
{
    public class PurchaseOrder
    {
        public int Id { get; set; }

        public string OrderNumber { get; set; } = string.Empty;

        public int SupplierId { get; set; }

        public Supplier Supplier { get; set; } = null!;

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public string Status { get; set; } = "Pending";

        public decimal TotalAmount { get; set; }

        public ICollection<PurchaseOrderItem> Items { get; set; }
            = new List<PurchaseOrderItem>();
    }
}
