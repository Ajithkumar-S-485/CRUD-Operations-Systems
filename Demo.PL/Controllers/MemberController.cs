using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IFeeRepository _feeRepository;

        public MemberController(IMemberRepository memberRepository, IFeeRepository feeRepository)
        {
            _memberRepository = memberRepository;
            _feeRepository = feeRepository;
        }

        // GET: Member
        public async Task<IActionResult> Index(string searchTerm)
        {
            var members = string.IsNullOrEmpty(searchTerm) 
                ? await _memberRepository.GetAllAsync() 
                : await _memberRepository.SearchMembersAsync(searchTerm);
            
            ViewBag.SearchTerm = searchTerm;
            return View(members);
        }

        // GET: Member/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var member = await _memberRepository.GetMemberWithDetailsAsync(id);
            if (member == null)
                return NotFound();

            return View(member);
        }

        // GET: Member/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Member/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Member member)
        {
            if (ModelState.IsValid)
            {
                member.JoinDate = DateTime.Now;
                member.IsActive = true;
                await _memberRepository.AddAsync(member);
                TempData["SuccessMessage"] = "Member created successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        // GET: Member/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var member = await _memberRepository.GetByIdAsync(id);
            if (member == null)
                return NotFound();

            return View(member);
        }

        // POST: Member/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Member member)
        {
            if (id != member.MemberId)
                return NotFound();

            if (ModelState.IsValid)
            {
                await _memberRepository.UpdateAsync(member);
                TempData["SuccessMessage"] = "Member updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        // GET: Member/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var member = await _memberRepository.GetByIdAsync(id);
            if (member == null)
                return NotFound();

            return View(member);
        }

        // POST: Member/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _memberRepository.DeleteAsync(id);
            TempData["SuccessMessage"] = "Member deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Member/Fees/5
        public async Task<IActionResult> Fees(int id)
        {
            var member = await _memberRepository.GetByIdAsync(id);
            if (member == null)
                return NotFound();

            var fees = await _feeRepository.GetFeesByMemberAsync(id);
            ViewBag.Member = member;
            return View(fees);
        }

        // POST: Member/ToggleStatus/5
        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var member = await _memberRepository.GetByIdAsync(id);
            if (member == null)
                return Json(new { success = false, message = "Member not found" });

            member.IsActive = !member.IsActive;
            await _memberRepository.UpdateAsync(member);

            return Json(new { 
                success = true, 
                isActive = member.IsActive,
                message = $"Member {(member.IsActive ? "activated" : "deactivated")} successfully" 
            });
        }
    }
}