namespace MiniERP.DTOs
{
    public record CreateCustomerDto(
      string Name,
      string Email,
      string Phone,
      string Address,
      decimal CreditLimit
  );

    public record UpdateCustomerDto(
        string Name,
        string Email,
        string Phone,
        string Address,
        decimal CreditLimit,
        bool IsActive
    );
}
