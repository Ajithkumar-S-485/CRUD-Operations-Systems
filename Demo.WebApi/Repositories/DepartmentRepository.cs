using Demo.WebApi.Data.Context;
using Demo.WebApi.Interfaces;
using Demo.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo.WebApi.Repositories
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Department?> GetByCodeAsync(string code)
        {
            return await _dbSet.FirstOrDefaultAsync(d => d.Code == code);
        }

        public async Task<IEnumerable<Department>> GetDepartmentsWithEmployeesAsync()
        {
            return await _dbSet.Include(d => d.Employees).ToListAsync();
        }

        public async Task<bool> CodeExistsAsync(string code, int? excludeId = null)
        {
            var query = _dbSet.Where(d => d.Code == code);
            if (excludeId.HasValue)
            {
                query = query.Where(d => d.Id != excludeId.Value);
            }
            return await query.AnyAsync();
        }

        public override async Task<IEnumerable<Department>> GetAllAsync()
        {
            return await _dbSet.Include(d => d.Employees).ToListAsync();
        }

        public override async Task<Department?> GetByIdAsync(int id)
        {
            return await _dbSet.Include(d => d.Employees).FirstOrDefaultAsync(d => d.Id == id);
        }
    }
}