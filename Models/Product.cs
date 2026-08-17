namespace MiniERP.Models
{

    public class Product
    {
        public int Id { get; set; }

        public string ProductCode { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public decimal CostPrice { get; set; }

        public int QuantityInStock { get; set; }

        public int ReorderLevel { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<SalesOrderItem> SalesOrderItems { get; set; }
            = new List<SalesOrderItem>();

        public ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; }
            = new List<PurchaseOrderItem>();
    }
}
