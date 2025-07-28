using Demo.BLL.Interfaces;
using Demo.DAL.Data;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class FeeRepository : GenericRepository<Fee>, IFeeRepository
    {
        public FeeRepository(FinAkhraDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Fee>> GetFeesByMemberAsync(int memberId)
        {
            return await _dbSet
                .Include(f => f.Member)
                .Where(f => f.MemberId == memberId)
                .OrderByDescending(f => f.FeeYear)
                .ThenByDescending(f => f.FeeMonth)
                .ToListAsync();
        }

        public async Task<IEnumerable<Fee>> GetFeesByYearAsync(int year)
        {
            return await _dbSet
                .Include(f => f.Member)
                .Where(f => f.FeeYear == year)
                .OrderBy(f => f.FeeMonth)
                .ThenBy(f => f.Member.FullName)
                .ToListAsync();
        }

        public async Task<IEnumerable<Fee>> GetFeesByMonthAndYearAsync(int month, int year)
        {
            return await _dbSet
                .Include(f => f.Member)
                .Where(f => f.FeeMonth == month && f.FeeYear == year)
                .OrderBy(f => f.Member.FullName)
                .ToListAsync();
        }

        public async Task<IEnumerable<Fee>> GetPendingFeesAsync()
        {
            return await _dbSet
                .Include(f => f.Member)
                .Where(f => f.PaidDate == null)
                .OrderBy(f => f.FeeYear)
                .ThenBy(f => f.FeeMonth)
                .ThenBy(f => f.Member.FullName)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalFeesCollectedAsync(int year)
        {
            return await _dbSet
                .Where(f => f.FeeYear == year && f.PaidDate != null && f.Amount.HasValue)
                .SumAsync(f => f.Amount.Value);
        }

        public async Task<decimal> GetTotalFeesCollectedByMonthAsync(int month, int year)
        {
            return await _dbSet
                .Where(f => f.FeeMonth == month && f.FeeYear == year && f.PaidDate != null && f.Amount.HasValue)
                .SumAsync(f => f.Amount.Value);
        }

        public async Task<bool> HasMemberPaidFeeAsync(int memberId, int month, int year)
        {
            return await _dbSet
                .AnyAsync(f => f.MemberId == memberId && 
                              f.FeeMonth == month && 
                              f.FeeYear == year && 
                              f.PaidDate != null);
        }
    }
}