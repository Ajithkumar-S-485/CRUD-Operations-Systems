using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class AccountingController : Controller
    {
        private readonly IAccountingRepository _accountingRepository;

        public AccountingController(IAccountingRepository accountingRepository)
        {
            _accountingRepository = accountingRepository;
        }

        // GET: Accounting
        public IActionResult Index()
        {
            return View();
        }

        // GET: Accounting/JournalEntries
        public async Task<IActionResult> JournalEntries()
        {
            var journalEntries = await _accountingRepository.GetJournalEntriesAsync();
            return View(journalEntries);
        }

        // GET: Accounting/CreateJournalEntry
        public async Task<IActionResult> CreateJournalEntry()
        {
            await PopulateAccounts();
            
            var journalEntry = new JournalEntry
            {
                EntryDate = DateTime.Today,
                CreatedBy = 1 // Should be from current user session
            };

            return View(journalEntry);
        }

        // POST: Accounting/CreateJournalEntry
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateJournalEntry(JournalEntry journalEntry, List<Transaction> transactions)
        {
            if (ModelState.IsValid && transactions != null && transactions.Count > 0)
            {
                // Validate double entry (debits = credits)
                decimal totalDebits = 0, totalCredits = 0;
                foreach (var transaction in transactions)
                {
                    totalDebits += transaction.DebitAmount;
                    totalCredits += transaction.CreditAmount;
                }

                if (totalDebits != totalCredits)
                {
                    ModelState.AddModelError("", "Total debits must equal total credits");
                    await PopulateAccounts();
                    return View(journalEntry);
                }

                await _accountingRepository.CreateJournalEntryAsync(journalEntry, transactions);
                TempData["SuccessMessage"] = "Journal entry created successfully!";
                return RedirectToAction(nameof(JournalEntries));
            }

            await PopulateAccounts();
            return View(journalEntry);
        }

        // GET: Accounting/LedgerAccounts
        public async Task<IActionResult> LedgerAccounts()
        {
            var accounts = await _accountingRepository.GetLedgerAccountsAsync();
            return View(accounts);
        }

        // GET: Accounting/AccountLedger/5
        public async Task<IActionResult> AccountLedger(int id)
        {
            var transactions = await _accountingRepository.GetTransactionsByAccountAsync(id);
            var balance = await _accountingRepository.GetAccountBalanceAsync(id);
            
            ViewBag.AccountBalance = balance;
            return View(transactions);
        }

        // GET: Accounting/CashBook
        public async Task<IActionResult> CashBook()
        {
            var cashBookEntries = await _accountingRepository.GetCashBookEntriesAsync();
            var cashBalance = await _accountingRepository.GetCashBalanceAsync();
            
            ViewBag.CashBalance = cashBalance;
            return View(cashBookEntries);
        }

        // GET: Accounting/AddCashEntry
        public IActionResult AddCashEntry()
        {
            var cashEntry = new CashBook
            {
                EntryDate = DateTime.Today
            };
            return View(cashEntry);
        }

        // POST: Accounting/AddCashEntry
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCashEntry(CashBook cashEntry)
        {
            if (ModelState.IsValid)
            {
                await _accountingRepository.AddCashBookEntryAsync(cashEntry);
                TempData["SuccessMessage"] = "Cash book entry added successfully!";
                return RedirectToAction(nameof(CashBook));
            }
            return View(cashEntry);
        }

        // GET: Accounting/Vouchers
        public async Task<IActionResult> Vouchers(string type = "all")
        {
            var vouchers = type == "all" 
                ? await _accountingRepository.GetVouchersAsync()
                : await _accountingRepository.GetVouchersByTypeAsync(type);

            ViewBag.VoucherType = type;
            return View(vouchers);
        }

        // GET: Accounting/CreateVoucher
        public async Task<IActionResult> CreateVoucher()
        {
            await PopulateVoucherDropdowns();
            
            var voucher = new Voucher
            {
                VoucherDate = DateTime.Today,
                VoucherType = "Receipt"
            };

            return View(voucher);
        }

        // POST: Accounting/CreateVoucher
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVoucher(Voucher voucher)
        {
            if (ModelState.IsValid)
            {
                await _accountingRepository.CreateVoucherAsync(voucher);
                TempData["SuccessMessage"] = "Voucher created successfully!";
                return RedirectToAction(nameof(Vouchers));
            }

            await PopulateVoucherDropdowns();
            return View(voucher);
        }

        // GET: Accounting/TrialBalance
        public async Task<IActionResult> TrialBalance()
        {
            var trialBalance = await _accountingRepository.GetTrialBalanceAsync();
            return View(trialBalance);
        }

        // GET: Accounting/IncomeStatement
        public async Task<IActionResult> IncomeStatement(DateTime? startDate, DateTime? endDate)
        {
            var start = startDate ?? DateTime.Today.AddMonths(-1);
            var end = endDate ?? DateTime.Today;

            var totalIncome = await _accountingRepository.GetTotalIncomeAsync(start, end);
            var totalExpense = await _accountingRepository.GetTotalExpenseAsync(start, end);
            var netIncome = totalIncome - totalExpense;

            ViewBag.StartDate = start;
            ViewBag.EndDate = end;
            ViewBag.TotalIncome = totalIncome;
            ViewBag.TotalExpense = totalExpense;
            ViewBag.NetIncome = netIncome;

            return View();
        }

        // GET: Accounting/Reports
        public async Task<IActionResult> Reports()
        {
            var cashBalance = await _accountingRepository.GetCashBalanceAsync();
            var currentMonthIncome = await _accountingRepository.GetTotalIncomeAsync(
                new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1), 
                DateTime.Today);
            var currentMonthExpense = await _accountingRepository.GetTotalExpenseAsync(
                new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1), 
                DateTime.Today);

            ViewBag.CashBalance = cashBalance;
            ViewBag.CurrentMonthIncome = currentMonthIncome;
            ViewBag.CurrentMonthExpense = currentMonthExpense;
            ViewBag.NetIncome = currentMonthIncome - currentMonthExpense;

            return View();
        }

        private async Task PopulateAccounts()
        {
            var accounts = await _accountingRepository.GetLedgerAccountsAsync();
            ViewBag.Accounts = new SelectList(accounts, "AccountId", "AccountName");
        }

        private async Task PopulateVoucherDropdowns()
        {
            var accounts = await _accountingRepository.GetLedgerAccountsAsync();
            ViewBag.Accounts = new SelectList(accounts, "AccountId", "AccountName");

            ViewBag.VoucherTypes = new SelectList(new[] 
            {
                new { Value = "Receipt", Text = "Receipt" },
                new { Value = "Payment", Text = "Payment" }
            }, "Value", "Text");
        }
    }
}