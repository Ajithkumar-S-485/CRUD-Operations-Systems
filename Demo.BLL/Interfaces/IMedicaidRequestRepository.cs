using Demo.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.BLL.Interfaces
{
    public interface IMedicaidRequestRepository : IGenericRepository<MedicaidRequest>
    {
        Task<IEnumerable<MedicaidRequest>> GetRequestsByMemberAsync(int memberId);
        Task<IEnumerable<MedicaidRequest>> GetRequestsByStatusAsync(string status);
        Task<IEnumerable<MedicaidRequest>> GetPendingRequestsAsync();
        Task<decimal> GetTotalApprovedAmountAsync();
        Task<decimal> GetTotalRequestedAmountAsync();
        Task<bool> UpdateRequestStatusAsync(int requestId, string status, decimal? approvedAmount = null);
    }
}