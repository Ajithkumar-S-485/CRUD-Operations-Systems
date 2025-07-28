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
    public class EventRepository : GenericRepository<Event>, IEventRepository
    {
        public EventRepository(FinAkhraDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Event>> GetEventsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Where(e => e.EventDate >= startDate && e.EventDate <= endDate)
                .OrderBy(e => e.EventDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetUpcomingEventsAsync()
        {
            var today = DateTime.Today;
            return await _dbSet
                .Where(e => e.EventDate >= today)
                .OrderBy(e => e.EventDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetPastEventsAsync()
        {
            var today = DateTime.Today;
            return await _dbSet
                .Where(e => e.EventDate < today)
                .OrderByDescending(e => e.EventDate)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalBudgetByYearAsync(int year)
        {
            return await _dbSet
                .Where(e => e.EventDate.Year == year && e.BudgetAmount.HasValue)
                .SumAsync(e => e.BudgetAmount.Value);
        }

        public async Task<decimal> GetTotalExpensesByYearAsync(int year)
        {
            return await _dbSet
                .Where(e => e.EventDate.Year == year && e.ActualExpense.HasValue)
                .SumAsync(e => e.ActualExpense.Value);
        }

        public async Task<IEnumerable<Event>> GetEventsOverBudgetAsync()
        {
            return await _dbSet
                .Where(e => e.BudgetAmount.HasValue && 
                           e.ActualExpense.HasValue && 
                           e.ActualExpense > e.BudgetAmount)
                .OrderByDescending(e => e.EventDate)
                .ToListAsync();
        }
    }
}