using Demo.BLL.Interfaces;
using Demo.DAL.Data;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class MedicaidRequestRepository : GenericRepository<MedicaidRequest>, IMedicaidRequestRepository
    {
        public MedicaidRequestRepository(FinAkhraDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<MedicaidRequest>> GetRequestsByMemberAsync(int memberId)
        {
            return await _dbSet
                .Include(mr => mr.Member)
                .Where(mr => mr.MemberId == memberId)
                .OrderByDescending(mr => mr.RequestDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<MedicaidRequest>> GetRequestsByStatusAsync(string status)
        {
            return await _dbSet
                .Include(mr => mr.Member)
                .Where(mr => mr.Status == status)
                .OrderByDescending(mr => mr.RequestDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<MedicaidRequest>> GetPendingRequestsAsync()
        {
            return await GetRequestsByStatusAsync("Pending");
        }

        public async Task<decimal> GetTotalApprovedAmountAsync()
        {
            return await _dbSet
                .Where(mr => mr.Status == "Approved" && mr.ApprovedAmount.HasValue)
                .SumAsync(mr => mr.ApprovedAmount.Value);
        }

        public async Task<decimal> GetTotalRequestedAmountAsync()
        {
            return await _dbSet
                .Where(mr => mr.AmountRequested.HasValue)
                .SumAsync(mr => mr.AmountRequested.Value);
        }

        public async Task<bool> UpdateRequestStatusAsync(int requestId, string status, decimal? approvedAmount = null)
        {
            var request = await _dbSet.FindAsync(requestId);
            if (request == null)
                return false;

            request.Status = status;
            if (approvedAmount.HasValue)
                request.ApprovedAmount = approvedAmount.Value;

            _context.Update(request);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}