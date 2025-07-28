using Demo.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.BLL.Interfaces
{
    public interface IFeeRepository : IGenericRepository<Fee>
    {
        Task<IEnumerable<Fee>> GetFeesByMemberAsync(int memberId);
        Task<IEnumerable<Fee>> GetFeesByYearAsync(int year);
        Task<IEnumerable<Fee>> GetFeesByMonthAndYearAsync(int month, int year);
        Task<IEnumerable<Fee>> GetPendingFeesAsync();
        Task<decimal> GetTotalFeesCollectedAsync(int year);
        Task<decimal> GetTotalFeesCollectedByMonthAsync(int month, int year);
        Task<bool> HasMemberPaidFeeAsync(int memberId, int month, int year);
    }
}