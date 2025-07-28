using Demo.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IFeeRepository _feeRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IMedicaidRequestRepository _medicaidRepository;
        private readonly IAccountingRepository _accountingRepository;

        public DashboardController(
            IMemberRepository memberRepository,
            IFeeRepository feeRepository,
            IEventRepository eventRepository,
            IMedicaidRequestRepository medicaidRepository,
            IAccountingRepository accountingRepository)
        {
            _memberRepository = memberRepository;
            _feeRepository = feeRepository;
            _eventRepository = eventRepository;
            _medicaidRepository = medicaidRepository;
            _accountingRepository = accountingRepository;
        }

        public async Task<IActionResult> Index()
        {
            var currentYear = DateTime.Now.Year;
            var currentMonth = DateTime.Now.Month;

            // Member statistics
            var totalMembers = await _memberRepository.CountAsync();
            var activeMembers = await _memberRepository.CountAsync(m => m.IsActive);

            // Fee statistics
            var totalFeesCollected = await _feeRepository.GetTotalFeesCollectedAsync(currentYear);
            var pendingFeesCount = await _feeRepository.CountAsync(f => f.PaidDate == null);

            // Event statistics
            var upcomingEvents = await _eventRepository.GetUpcomingEventsAsync();
            var totalBudget = await _eventRepository.GetTotalBudgetByYearAsync(currentYear);

            // Medicaid statistics
            var pendingMedicaidRequests = await _medicaidRepository.CountAsync(r => r.Status == "Pending");
            var totalApproved = await _medicaidRepository.GetTotalApprovedAmountAsync();

            // Financial statistics
            var cashBalance = await _accountingRepository.GetCashBalanceAsync();
            var monthlyIncome = await _accountingRepository.GetTotalIncomeAsync(
                new DateTime(currentYear, currentMonth, 1), 
                DateTime.Today);
            var monthlyExpense = await _accountingRepository.GetTotalExpenseAsync(
                new DateTime(currentYear, currentMonth, 1), 
                DateTime.Today);

            ViewBag.TotalMembers = totalMembers;
            ViewBag.ActiveMembers = activeMembers;
            ViewBag.TotalFeesCollected = totalFeesCollected;
            ViewBag.PendingFeesCount = pendingFeesCount;
            ViewBag.UpcomingEventsCount = upcomingEvents.Count();
            ViewBag.TotalBudget = totalBudget;
            ViewBag.PendingMedicaidRequests = pendingMedicaidRequests;
            ViewBag.TotalApproved = totalApproved;
            ViewBag.CashBalance = cashBalance;
            ViewBag.MonthlyIncome = monthlyIncome;
            ViewBag.MonthlyExpense = monthlyExpense;
            ViewBag.NetIncome = monthlyIncome - monthlyExpense;

            return View();
        }

        public async Task<IActionResult> QuickStats()
        {
            var currentYear = DateTime.Now.Year;
            var stats = new
            {
                TotalMembers = await _memberRepository.CountAsync(),
                ActiveMembers = await _memberRepository.CountAsync(m => m.IsActive),
                PendingFees = await _feeRepository.CountAsync(f => f.PaidDate == null),
                PendingMedicaid = await _medicaidRepository.CountAsync(r => r.Status == "Pending"),
                CashBalance = await _accountingRepository.GetCashBalanceAsync(),
                YearlyFeesCollected = await _feeRepository.GetTotalFeesCollectedAsync(currentYear)
            };

            return Json(stats);
        }
    }
}