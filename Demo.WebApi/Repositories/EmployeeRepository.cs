using Demo.WebApi.Data.Context;
using Demo.WebApi.Interfaces;
using Demo.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo.WebApi.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByDepartmentAsync(int departmentId)
        {
            return await _dbSet
                .Where(e => e.DepartmentId == departmentId && !e.IsDeleted)
                .Include(e => e.Department)
                .ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetActiveEmployeesAsync()
        {
            return await _dbSet
                .Where(e => e.IsActive && !e.IsDeleted)
                .Include(e => e.Department)
                .ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetEmployeesWithDepartmentAsync()
        {
            return await _dbSet
                .Where(e => !e.IsDeleted)
                .Include(e => e.Department)
                .ToListAsync();
        }

        public async Task<Employee?> GetByIdWithDepartmentAsync(int id)
        {
            return await _dbSet
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
        }

        public async Task<bool> EmailExistsAsync(string email, int? excludeId = null)
        {
            var query = _dbSet.Where(e => e.Email == email && !e.IsDeleted);
            if (excludeId.HasValue)
            {
                query = query.Where(e => e.Id != excludeId.Value);
            }
            return await query.AnyAsync();
        }

        public override async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _dbSet
                .Where(e => !e.IsDeleted)
                .Include(e => e.Department)
                .ToListAsync();
        }

        public override async Task<Employee?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
        }

        public override async Task DeleteAsync(int id)
        {
            var employee = await _dbSet.FindAsync(id);
            if (employee != null)
            {
                employee.IsDeleted = true;
                _dbSet.Update(employee);
            }
        }
    }
}