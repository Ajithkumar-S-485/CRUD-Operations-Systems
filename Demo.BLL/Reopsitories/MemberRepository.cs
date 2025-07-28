using Demo.BLL.Interfaces;
using Demo.DAL.Data;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class MemberRepository : GenericRepository<Member>, IMemberRepository
    {
        public MemberRepository(FinAkhraDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Member>> GetActiveMembersAsync()
        {
            return await _dbSet
                .Where(m => m.IsActive)
                .OrderBy(m => m.FullName)
                .ToListAsync();
        }

        public async Task<IEnumerable<Member>> GetMembersWithFeesAsync()
        {
            return await _dbSet
                .Include(m => m.Fees)
                .OrderBy(m => m.FullName)
                .ToListAsync();
        }

        public async Task<Member> GetMemberWithDetailsAsync(int memberId)
        {
            return await _dbSet
                .Include(m => m.Fees)
                .Include(m => m.MedicaidRequests)
                .Include(m => m.Vouchers)
                .FirstOrDefaultAsync(m => m.MemberId == memberId);
        }

        public async Task<bool> IsMemberActiveAsync(int memberId)
        {
            var member = await _dbSet.FindAsync(memberId);
            return member?.IsActive ?? false;
        }

        public async Task<IEnumerable<Member>> SearchMembersAsync(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return await GetAllAsync();

            searchTerm = searchTerm.ToLower();
            return await _dbSet
                .Where(m => m.FullName.ToLower().Contains(searchTerm) ||
                           m.Email.ToLower().Contains(searchTerm) ||
                           m.Mobile.Contains(searchTerm))
                .OrderBy(m => m.FullName)
                .ToListAsync();
        }
    }
}