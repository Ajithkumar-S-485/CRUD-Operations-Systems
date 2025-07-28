using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class EventController : Controller
    {
        private readonly IEventRepository _eventRepository;

        public EventController(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        // GET: Event
        public async Task<IActionResult> Index(string filter = "all")
        {
            var events = filter switch
            {
                "upcoming" => await _eventRepository.GetUpcomingEventsAsync(),
                "past" => await _eventRepository.GetPastEventsAsync(),
                _ => await _eventRepository.GetAllAsync()
            };

            ViewBag.Filter = filter;
            return View(events);
        }

        // GET: Event/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var eventItem = await _eventRepository.GetByIdAsync(id);
            if (eventItem == null)
                return NotFound();

            return View(eventItem);
        }

        // GET: Event/Create
        public IActionResult Create()
        {
            var eventItem = new Event
            {
                EventDate = DateTime.Today.AddDays(1)
            };
            return View(eventItem);
        }

        // POST: Event/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event eventItem)
        {
            if (ModelState.IsValid)
            {
                await _eventRepository.AddAsync(eventItem);
                TempData["SuccessMessage"] = "Event created successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(eventItem);
        }

        // GET: Event/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var eventItem = await _eventRepository.GetByIdAsync(id);
            if (eventItem == null)
                return NotFound();

            return View(eventItem);
        }

        // POST: Event/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Event eventItem)
        {
            if (id != eventItem.EventId)
                return NotFound();

            if (ModelState.IsValid)
            {
                await _eventRepository.UpdateAsync(eventItem);
                TempData["SuccessMessage"] = "Event updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(eventItem);
        }

        // GET: Event/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var eventItem = await _eventRepository.GetByIdAsync(id);
            if (eventItem == null)
                return NotFound();

            return View(eventItem);
        }

        // POST: Event/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _eventRepository.DeleteAsync(id);
            TempData["SuccessMessage"] = "Event deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Event/Budget
        public async Task<IActionResult> Budget(int? year)
        {
            var currentYear = DateTime.Now.Year;
            var selectedYear = year ?? currentYear;

            var totalBudget = await _eventRepository.GetTotalBudgetByYearAsync(selectedYear);
            var totalExpenses = await _eventRepository.GetTotalExpensesByYearAsync(selectedYear);
            var overBudgetEvents = await _eventRepository.GetEventsOverBudgetAsync();

            ViewBag.TotalBudget = totalBudget;
            ViewBag.TotalExpenses = totalExpenses;
            ViewBag.BudgetVariance = totalBudget - totalExpenses;
            ViewBag.OverBudgetEvents = overBudgetEvents;
            ViewBag.SelectedYear = selectedYear;

            return View();
        }

        // GET: Event/Calendar
        public async Task<IActionResult> Calendar(int? year, int? month)
        {
            var currentDate = DateTime.Now;
            var selectedYear = year ?? currentDate.Year;
            var selectedMonth = month ?? currentDate.Month;

            var startDate = new DateTime(selectedYear, selectedMonth, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            var events = await _eventRepository.GetEventsByDateRangeAsync(startDate, endDate);

            ViewBag.SelectedYear = selectedYear;
            ViewBag.SelectedMonth = selectedMonth;
            ViewBag.MonthName = startDate.ToString("MMMM yyyy");

            return View(events);
        }

        // POST: Event/UpdateExpense/5
        [HttpPost]
        public async Task<IActionResult> UpdateExpense(int id, decimal actualExpense)
        {
            var eventItem = await _eventRepository.GetByIdAsync(id);
            if (eventItem == null)
                return Json(new { success = false, message = "Event not found" });

            eventItem.ActualExpense = actualExpense;
            await _eventRepository.UpdateAsync(eventItem);

            var variance = (eventItem.BudgetAmount ?? 0) - actualExpense;
            var isOverBudget = variance < 0;

            return Json(new { 
                success = true, 
                message = "Actual expense updated successfully",
                variance = variance,
                isOverBudget = isOverBudget
            });
        }
    }
}