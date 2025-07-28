using Demo.DAL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.BLL.Interfaces
{
    public interface IEventRepository : IGenericRepository<Event>
    {
        Task<IEnumerable<Event>> GetEventsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Event>> GetUpcomingEventsAsync();
        Task<IEnumerable<Event>> GetPastEventsAsync();
        Task<decimal> GetTotalBudgetByYearAsync(int year);
        Task<decimal> GetTotalExpensesByYearAsync(int year);
        Task<IEnumerable<Event>> GetEventsOverBudgetAsync();
    }
}