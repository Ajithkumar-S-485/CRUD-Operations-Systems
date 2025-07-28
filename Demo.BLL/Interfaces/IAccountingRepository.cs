using Demo.DAL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.BLL.Interfaces
{
    public interface IAccountingRepository
    {
        // Journal Entry operations
        Task<IEnumerable<JournalEntry>> GetJournalEntriesAsync();
        Task<JournalEntry> CreateJournalEntryAsync(JournalEntry journalEntry, List<Transaction> transactions);
        
        // Ledger Account operations
        Task<IEnumerable<LedgerAccount>> GetLedgerAccountsAsync();
        Task<IEnumerable<LedgerAccount>> GetCashAccountsAsync();
        Task<decimal> GetAccountBalanceAsync(int accountId);
        
        // Transaction operations
        Task<IEnumerable<Transaction>> GetTransactionsByAccountAsync(int accountId);
        Task<IEnumerable<Transaction>> GetTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate);
        
        // Cash Book operations
        Task<IEnumerable<CashBook>> GetCashBookEntriesAsync();
        Task<CashBook> AddCashBookEntryAsync(CashBook cashBookEntry);
        Task<decimal> GetCashBalanceAsync();
        
        // Voucher operations
        Task<IEnumerable<Voucher>> GetVouchersAsync();
        Task<IEnumerable<Voucher>> GetVouchersByTypeAsync(string voucherType);
        Task<Voucher> CreateVoucherAsync(Voucher voucher);
        
        // Reports
        Task<decimal> GetTotalIncomeAsync(DateTime startDate, DateTime endDate);
        Task<decimal> GetTotalExpenseAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<object>> GetTrialBalanceAsync();
    }
}