using Microsoft.AspNetCore.Mvc;
using MiniERP.Data;
using MiniERP.DTOs;
using MiniERP.Models;
using Microsoft.EntityFrameworkCore;
namespace MiniERP.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class GeneralLedgerController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GeneralLedgerController(
            AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetLedger()
        {
            var entries = await _context.JournalEntries
                .AsNoTracking()
                .OrderBy(x => x.TransactionDate)
                .ThenBy(x => x.Id)
                .ToListAsync();

            return Ok(entries);
        }

        [HttpGet("account/{accountCode}")]
        public async Task<IActionResult> GetAccount(
            string accountCode)
        {
            var entries = await _context.JournalEntries
                .Where(x => x.AccountCode == accountCode)
                .AsNoTracking()
                .OrderBy(x => x.TransactionDate)
                .ToListAsync();

            if (!entries.Any())
                return NotFound(
                    $"No entries found for account {accountCode}.");

            var debit = entries.Sum(x => x.Debit);
            var credit = entries.Sum(x => x.Credit);

            return Ok(new
            {
                accountCode,
                accountName = entries.First().AccountName,
                totalDebit = debit,
                totalCredit = credit,
                balance = debit - credit,
                entries
            });
        }

        [HttpGet("trial-balance")]
        public async Task<IActionResult> GetTrialBalance()
        {
            var trialBalanceData = await _context.JournalEntries
     .GroupBy(j => new
     {
         j.AccountCode,
         j.AccountName
     })
     .Select(g => new
     {
         AccountCode = g.Key.AccountCode,
         AccountName = g.Key.AccountName,
         Debit = g.Sum(x => x.Debit),
         Credit = g.Sum(x => x.Credit)
     })
     .OrderBy(x => x.AccountCode)
     .ToListAsync();

            var trialBalance = trialBalanceData
                .Select(x => new TrialBalanceDto(
                    x.AccountCode,
                    x.AccountName,
                    x.Debit,
                    x.Credit
                ))
                .ToList();

            return Ok(trialBalance);
        }

        [HttpGet("profit-loss")]
        public async Task<IActionResult> GetProfitAndLoss()
        {
            var revenue = await _context.JournalEntries
                .Where(x => x.AccountCode.StartsWith("4"))
                .SumAsync(x => x.Credit - x.Debit);

            var expenses = await _context.JournalEntries
                .Where(x => x.AccountCode.StartsWith("5"))
                .SumAsync(x => x.Debit - x.Credit);

            return Ok(new
            {
                revenue,
                expenses,
                profit = revenue - expenses
            });
        }
    }
}
