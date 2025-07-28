using Demo.WebApi.Models;

namespace Demo.WebApi.Interfaces
{
    public interface IDepartmentRepository : IGenericRepository<Department>
    {
        Task<Department?> GetByCodeAsync(string code);
        Task<IEnumerable<Department>> GetDepartmentsWithEmployeesAsync();
        Task<bool> CodeExistsAsync(string code, int? excludeId = null);
    }
}