using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class MedicaidController : Controller
    {
        private readonly IMedicaidRequestRepository _medicaidRepository;
        private readonly IMemberRepository _memberRepository;

        public MedicaidController(IMedicaidRequestRepository medicaidRepository, IMemberRepository memberRepository)
        {
            _medicaidRepository = medicaidRepository;
            _memberRepository = memberRepository;
        }

        // GET: Medicaid
        public async Task<IActionResult> Index(string status = "all")
        {
            var requests = status == "all" 
                ? await _medicaidRepository.GetAllAsync()
                : await _medicaidRepository.GetRequestsByStatusAsync(status);

            ViewBag.Status = status;
            ViewBag.StatusList = new SelectList(new[] { 
                new { Value = "all", Text = "All Requests" },
                new { Value = "Pending", Text = "Pending" },
                new { Value = "Approved", Text = "Approved" },
                new { Value = "Rejected", Text = "Rejected" }
            }, "Value", "Text", status);

            return View(requests);
        }

        // GET: Medicaid/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var request = await _medicaidRepository.GetByIdAsync(id);
            if (request == null)
                return NotFound();

            return View(request);
        }

        // GET: Medicaid/Create
        public async Task<IActionResult> Create(int? memberId)
        {
            await PopulateMembers();
            
            var request = new MedicaidRequest
            {
                RequestDate = DateTime.Today,
                Status = "Pending"
            };

            if (memberId.HasValue)
                request.MemberId = memberId.Value;

            return View(request);
        }

        // POST: Medicaid/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MedicaidRequest request)
        {
            if (ModelState.IsValid)
            {
                request.Status = "Pending";
                await _medicaidRepository.AddAsync(request);
                TempData["SuccessMessage"] = "Medicaid request submitted successfully!";
                return RedirectToAction(nameof(Index));
            }

            await PopulateMembers();
            return View(request);
        }

        // GET: Medicaid/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var request = await _medicaidRepository.GetByIdAsync(id);
            if (request == null)
                return NotFound();

            await PopulateMembers();
            return View(request);
        }

        // POST: Medicaid/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MedicaidRequest request)
        {
            if (id != request.RequestId)
                return NotFound();

            if (ModelState.IsValid)
            {
                await _medicaidRepository.UpdateAsync(request);
                TempData["SuccessMessage"] = "Medicaid request updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            await PopulateMembers();
            return View(request);
        }

        // GET: Medicaid/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var request = await _medicaidRepository.GetByIdAsync(id);
            if (request == null)
                return NotFound();

            return View(request);
        }

        // POST: Medicaid/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _medicaidRepository.DeleteAsync(id);
            TempData["SuccessMessage"] = "Medicaid request deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Medicaid/Pending
        public async Task<IActionResult> Pending()
        {
            var pendingRequests = await _medicaidRepository.GetPendingRequestsAsync();
            return View(pendingRequests);
        }

        // POST: Medicaid/Approve/5
        [HttpPost]
        public async Task<IActionResult> Approve(int id, decimal approvedAmount)
        {
            var success = await _medicaidRepository.UpdateRequestStatusAsync(id, "Approved", approvedAmount);
            
            if (success)
            {
                return Json(new { 
                    success = true, 
                    message = "Request approved successfully",
                    approvedAmount = approvedAmount
                });
            }
            
            return Json(new { success = false, message = "Failed to approve request" });
        }

        // POST: Medicaid/Reject/5
        [HttpPost]
        public async Task<IActionResult> Reject(int id)
        {
            var success = await _medicaidRepository.UpdateRequestStatusAsync(id, "Rejected");
            
            if (success)
            {
                return Json(new { 
                    success = true, 
                    message = "Request rejected successfully"
                });
            }
            
            return Json(new { success = false, message = "Failed to reject request" });
        }

        // GET: Medicaid/Reports
        public async Task<IActionResult> Reports()
        {
            var totalRequested = await _medicaidRepository.GetTotalRequestedAmountAsync();
            var totalApproved = await _medicaidRepository.GetTotalApprovedAmountAsync();
            var pendingRequests = await _medicaidRepository.GetPendingRequestsAsync();

            ViewBag.TotalRequested = totalRequested;
            ViewBag.TotalApproved = totalApproved;
            ViewBag.PendingCount = await _medicaidRepository.CountAsync(r => r.Status == "Pending");

            return View();
        }

        private async Task PopulateMembers()
        {
            var members = await _memberRepository.GetActiveMembersAsync();
            ViewBag.Members = new SelectList(members, "MemberId", "FullName");
        }
    }
}