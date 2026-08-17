namespace MiniERP.DTOs
{
    public record TrialBalanceDto(
      string AccountCode,
      string AccountName,
      decimal Debit,
      decimal Credit
  );
}
