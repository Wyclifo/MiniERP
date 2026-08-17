namespace MiniERP.DTOs
{
    public record CreatePaymentDto(
    int InvoiceId,
    decimal Amount,
    string PaymentMethod,
    string Reference
);
}
