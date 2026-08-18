namespace MiniERP.Models
{
    public class PurchaseOrderItem
    {
        public int Id { get; set; }

        public int PurchaseOrderId { get; set; }

        public PurchaseOrder PurchaseOrder { get; set; } = null!;

        public int ProductId { get; set; }

        public Product Product { get; set; } = null!;

        public int Quantity { get; set; }

        public decimal UnitCost { get; set; }

        public decimal Total => Quantity * UnitCost;
    }
}
