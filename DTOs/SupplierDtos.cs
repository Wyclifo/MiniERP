namespace MiniERP.DTOs
{
    public record CreateSupplierDto(
     string Name,
     string Email,
     string Phone,
     string Address
 );

    public record UpdateSupplierDto(
        string Name,
        string Email,
        string Phone,
        string Address,
        bool IsActive
    );
}
