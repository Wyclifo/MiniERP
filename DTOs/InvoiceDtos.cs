namespace MiniERP.DTOs
{
    public record CreateInvoiceDto(
    int SalesOrderId,
    int PaymentTermDays = 30
);  
}
