namespace MiniERP.Models
{
    public class SalesOrderItem
    {
        public int Id { get; set; }

        public int SalesOrderId { get; set; }

        public SalesOrder SalesOrder { get; set; } = null!;

        public int ProductId { get; set; }

        public Product Product { get; set; } = null!;

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Total =>
            Quantity * UnitPrice;
    }
}
