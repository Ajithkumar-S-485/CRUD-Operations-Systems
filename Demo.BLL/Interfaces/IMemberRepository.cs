using Demo.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.BLL.Interfaces
{
    public interface IMemberRepository : IGenericRepository<Member>
    {
        Task<IEnumerable<Member>> GetActiveMembersAsync();
        Task<IEnumerable<Member>> GetMembersWithFeesAsync();
        Task<Member> GetMemberWithDetailsAsync(int memberId);
        Task<bool> IsMemberActiveAsync(int memberId);
        Task<IEnumerable<Member>> SearchMembersAsync(string searchTerm);
    }
}