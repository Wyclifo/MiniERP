namespace MiniERP.DTOs
{
    public record SalesOrderItemDto(
      int ProductId,
      int Quantity
  );

    public record CreateSalesOrderDto(
        int CustomerId,
        List<SalesOrderItemDto> Items
    );
}
