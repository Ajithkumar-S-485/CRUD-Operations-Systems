using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class FeeController : Controller
    {
        private readonly IFeeRepository _feeRepository;
        private readonly IMemberRepository _memberRepository;

        public FeeController(IFeeRepository feeRepository, IMemberRepository memberRepository)
        {
            _feeRepository = feeRepository;
            _memberRepository = memberRepository;
        }

        // GET: Fee
        public async Task<IActionResult> Index(int? year, int? month)
        {
            var currentYear = DateTime.Now.Year;
            var fees = year.HasValue && month.HasValue 
                ? await _feeRepository.GetFeesByMonthAndYearAsync(month.Value, year.Value)
                : await _feeRepository.GetFeesByYearAsync(year ?? currentYear);

            ViewBag.Years = Enumerable.Range(currentYear - 5, 10).Select(y => new SelectListItem 
            { 
                Value = y.ToString(), 
                Text = y.ToString(),
                Selected = y == (year ?? currentYear)
            });

            ViewBag.Months = Enumerable.Range(1, 12).Select(m => new SelectListItem 
            { 
                Value = m.ToString(), 
                Text = new DateTime(2000, m, 1).ToString("MMMM"),
                Selected = m == month
            });

            ViewBag.SelectedYear = year ?? currentYear;
            ViewBag.SelectedMonth = month;

            return View(fees);
        }

        // GET: Fee/Create
        public async Task<IActionResult> Create(int? memberId)
        {
            await PopulateDropdowns();
            
            var fee = new Fee
            {
                FeeMonth = DateTime.Now.Month,
                FeeYear = DateTime.Now.Year
            };

            if (memberId.HasValue)
                fee.MemberId = memberId.Value;

            return View(fee);
        }

        // POST: Fee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Fee fee)
        {
            if (ModelState.IsValid)
            {
                // Check if fee already exists for this member, month, and year
                var existingFee = await _feeRepository.HasMemberPaidFeeAsync(fee.MemberId, fee.FeeMonth ?? 0, fee.FeeYear ?? 0);
                if (existingFee)
                {
                    ModelState.AddModelError("", "Fee for this member, month, and year already exists.");
                    await PopulateDropdowns();
                    return View(fee);
                }

                if (fee.PaidDate.HasValue)
                    fee.ReceiptNumber = GenerateReceiptNumber();

                await _feeRepository.AddAsync(fee);
                TempData["SuccessMessage"] = "Fee record created successfully!";
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdowns();
            return View(fee);
        }

        // GET: Fee/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var fee = await _feeRepository.GetByIdAsync(id);
            if (fee == null)
                return NotFound();

            await PopulateDropdowns();
            return View(fee);
        }

        // POST: Fee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Fee fee)
        {
            if (id != fee.FeeId)
                return NotFound();

            if (ModelState.IsValid)
            {
                if (fee.PaidDate.HasValue && string.IsNullOrEmpty(fee.ReceiptNumber))
                    fee.ReceiptNumber = GenerateReceiptNumber();

                await _feeRepository.UpdateAsync(fee);
                TempData["SuccessMessage"] = "Fee record updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdowns();
            return View(fee);
        }

        // GET: Fee/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var fee = await _feeRepository.GetByIdAsync(id);
            if (fee == null)
                return NotFound();

            return View(fee);
        }

        // POST: Fee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _feeRepository.DeleteAsync(id);
            TempData["SuccessMessage"] = "Fee record deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Fee/Pending
        public async Task<IActionResult> Pending()
        {
            var pendingFees = await _feeRepository.GetPendingFeesAsync();
            return View(pendingFees);
        }

        // POST: Fee/MarkAsPaid/5
        [HttpPost]
        public async Task<IActionResult> MarkAsPaid(int id, decimal amount)
        {
            var fee = await _feeRepository.GetByIdAsync(id);
            if (fee == null)
                return Json(new { success = false, message = "Fee record not found" });

            fee.PaidDate = DateTime.Now;
            fee.Amount = amount;
            fee.ReceiptNumber = GenerateReceiptNumber();

            await _feeRepository.UpdateAsync(fee);

            return Json(new { 
                success = true, 
                message = "Fee marked as paid successfully",
                receiptNumber = fee.ReceiptNumber
            });
        }

        // GET: Fee/Reports
        public async Task<IActionResult> Reports(int? year)
        {
            var currentYear = DateTime.Now.Year;
            var selectedYear = year ?? currentYear;

            var totalCollected = await _feeRepository.GetTotalFeesCollectedAsync(selectedYear);
            
            ViewBag.TotalCollected = totalCollected;
            ViewBag.SelectedYear = selectedYear;
            ViewBag.Years = Enumerable.Range(currentYear - 5, 10).Select(y => new SelectListItem 
            { 
                Value = y.ToString(), 
                Text = y.ToString(),
                Selected = y == selectedYear
            });

            return View();
        }

        private async Task PopulateDropdowns()
        {
            var members = await _memberRepository.GetActiveMembersAsync();
            ViewBag.Members = members.Select(m => new SelectListItem 
            { 
                Value = m.MemberId.ToString(), 
                Text = m.FullName 
            });

            ViewBag.Months = Enumerable.Range(1, 12).Select(m => new SelectListItem 
            { 
                Value = m.ToString(), 
                Text = new DateTime(2000, m, 1).ToString("MMMM") 
            });

            var currentYear = DateTime.Now.Year;
            ViewBag.Years = Enumerable.Range(currentYear - 2, 5).Select(y => new SelectListItem 
            { 
                Value = y.ToString(), 
                Text = y.ToString() 
            });
        }

        private string GenerateReceiptNumber()
        {
            return "RCP" + DateTime.Now.ToString("yyyyMMddHHmmss");
        }
    }
}