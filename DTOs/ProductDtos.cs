namespace MiniERP.DTOs
{
    public record CreateProductDto(
      string ProductCode,
      string Name,
      string Description,
      decimal Price,
      decimal CostPrice,
      int QuantityInStock,
      int ReorderLevel
  );

    public record UpdateProductDto(
        string Name,
        string Description,
        decimal Price,
        decimal CostPrice,
        int QuantityInStock,
        int ReorderLevel,
        bool IsActive
    );
}
