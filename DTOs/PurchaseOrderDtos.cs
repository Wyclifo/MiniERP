namespace MiniERP.DTOs
{
    public record PurchaseOrderItemDto(
     int ProductId,
     int Quantity,
     decimal UnitCost
 );

    public record CreatePurchaseOrderDto(
        int SupplierId,
        List<PurchaseOrderItemDto> Items
    );
}
