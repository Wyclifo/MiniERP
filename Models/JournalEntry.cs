namespace MiniERP.Models
{
    public class JournalEntry
    {
        public int Id { get; set; }

        public string Reference { get; set; } = string.Empty;

        public string AccountCode { get; set; } = string.Empty;

        public string AccountName { get; set; } = string.Empty;

        public decimal Debit { get; set; }

        public decimal Credit { get; set; }

        public string Description { get; set; } = string.Empty;

        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
    }
}
