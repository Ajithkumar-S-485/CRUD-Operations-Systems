using Demo.BLL.Interfaces;
using Demo.DAL.Data;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class AccountingRepository : IAccountingRepository
    {
        private readonly FinAkhraDbContext _context;

        public AccountingRepository(FinAkhraDbContext context)
        {
            _context = context;
        }

        // Journal Entry operations
        public async Task<IEnumerable<JournalEntry>> GetJournalEntriesAsync()
        {
            return await _context.JournalEntries
                .Include(je => je.CreatedByUser)
                .Include(je => je.Transactions)
                    .ThenInclude(t => t.LedgerAccount)
                .OrderByDescending(je => je.EntryDate)
                .ToListAsync();
        }

        public async Task<JournalEntry> CreateJournalEntryAsync(JournalEntry journalEntry, List<Transaction> transactions)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Add journal entry
                _context.JournalEntries.Add(journalEntry);
                await _context.SaveChangesAsync();

                // Add transactions
                foreach (var trans in transactions)
                {
                    trans.JournalId = journalEntry.JournalId;
                    _context.Transactions.Add(trans);
                }
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return journalEntry;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // Ledger Account operations
        public async Task<IEnumerable<LedgerAccount>> GetLedgerAccountsAsync()
        {
            return await _context.LedgerAccounts
                .OrderBy(la => la.AccountType)
                .ThenBy(la => la.AccountName)
                .ToListAsync();
        }

        public async Task<IEnumerable<LedgerAccount>> GetCashAccountsAsync()
        {
            return await _context.LedgerAccounts
                .Where(la => la.IsCashAccount)
                .OrderBy(la => la.AccountName)
                .ToListAsync();
        }

        public async Task<decimal> GetAccountBalanceAsync(int accountId)
        {
            var debitTotal = await _context.Transactions
                .Where(t => t.AccountId == accountId)
                .SumAsync(t => t.DebitAmount);

            var creditTotal = await _context.Transactions
                .Where(t => t.AccountId == accountId)
                .SumAsync(t => t.CreditAmount);

            return debitTotal - creditTotal;
        }

        // Transaction operations
        public async Task<IEnumerable<Transaction>> GetTransactionsByAccountAsync(int accountId)
        {
            return await _context.Transactions
                .Include(t => t.JournalEntry)
                .Include(t => t.LedgerAccount)
                .Where(t => t.AccountId == accountId)
                .OrderByDescending(t => t.JournalEntry.EntryDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Transactions
                .Include(t => t.JournalEntry)
                .Include(t => t.LedgerAccount)
                .Where(t => t.JournalEntry.EntryDate >= startDate && t.JournalEntry.EntryDate <= endDate)
                .OrderByDescending(t => t.JournalEntry.EntryDate)
                .ToListAsync();
        }

        // Cash Book operations
        public async Task<IEnumerable<CashBook>> GetCashBookEntriesAsync()
        {
            return await _context.CashBooks
                .OrderByDescending(cb => cb.EntryDate)
                .ToListAsync();
        }

        public async Task<CashBook> AddCashBookEntryAsync(CashBook cashBookEntry)
        {
            // Calculate balance based on previous entries
            var lastEntry = await _context.CashBooks
                .OrderByDescending(cb => cb.EntryDate)
                .ThenByDescending(cb => cb.CashBookId)
                .FirstOrDefaultAsync();

            var previousBalance = lastEntry?.Balance ?? 0;
            cashBookEntry.Balance = previousBalance + (cashBookEntry.Debit ?? 0) - (cashBookEntry.Credit ?? 0);

            _context.CashBooks.Add(cashBookEntry);
            await _context.SaveChangesAsync();
            return cashBookEntry;
        }

        public async Task<decimal> GetCashBalanceAsync()
        {
            var lastEntry = await _context.CashBooks
                .OrderByDescending(cb => cb.EntryDate)
                .ThenByDescending(cb => cb.CashBookId)
                .FirstOrDefaultAsync();

            return lastEntry?.Balance ?? 0;
        }

        // Voucher operations
        public async Task<IEnumerable<Voucher>> GetVouchersAsync()
        {
            return await _context.Vouchers
                .Include(v => v.Member)
                .Include(v => v.LedgerAccount)
                .Include(v => v.JournalEntry)
                .OrderByDescending(v => v.VoucherDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Voucher>> GetVouchersByTypeAsync(string voucherType)
        {
            return await _context.Vouchers
                .Include(v => v.Member)
                .Include(v => v.LedgerAccount)
                .Include(v => v.JournalEntry)
                .Where(v => v.VoucherType == voucherType)
                .OrderByDescending(v => v.VoucherDate)
                .ToListAsync();
        }

        public async Task<Voucher> CreateVoucherAsync(Voucher voucher)
        {
            _context.Vouchers.Add(voucher);
            await _context.SaveChangesAsync();
            return voucher;
        }

        // Reports
        public async Task<decimal> GetTotalIncomeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Transactions
                .Include(t => t.LedgerAccount)
                .Include(t => t.JournalEntry)
                .Where(t => t.LedgerAccount.AccountType == "Income" &&
                           t.JournalEntry.EntryDate >= startDate &&
                           t.JournalEntry.EntryDate <= endDate)
                .SumAsync(t => t.CreditAmount);
        }

        public async Task<decimal> GetTotalExpenseAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Transactions
                .Include(t => t.LedgerAccount)
                .Include(t => t.JournalEntry)
                .Where(t => t.LedgerAccount.AccountType == "Expense" &&
                           t.JournalEntry.EntryDate >= startDate &&
                           t.JournalEntry.EntryDate <= endDate)
                .SumAsync(t => t.DebitAmount);
        }

        public async Task<IEnumerable<object>> GetTrialBalanceAsync()
        {
            var accounts = await _context.LedgerAccounts.ToListAsync();
            var trialBalance = new List<object>();

            foreach (var account in accounts)
            {
                var debitTotal = await _context.Transactions
                    .Where(t => t.AccountId == account.AccountId)
                    .SumAsync(t => t.DebitAmount);

                var creditTotal = await _context.Transactions
                    .Where(t => t.AccountId == account.AccountId)
                    .SumAsync(t => t.CreditAmount);

                trialBalance.Add(new
                {
                    AccountId = account.AccountId,
                    AccountName = account.AccountName,
                    AccountType = account.AccountType,
                    DebitTotal = debitTotal,
                    CreditTotal = creditTotal,
                    Balance = debitTotal - creditTotal
                });
            }

            return trialBalance;
        }
    }
}