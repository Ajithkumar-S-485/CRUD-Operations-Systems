using Demo.WebApi.Models;

namespace Demo.WebApi.Interfaces
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        Task<IEnumerable<Employee>> GetEmployeesByDepartmentAsync(int departmentId);
        Task<IEnumerable<Employee>> GetActiveEmployeesAsync();
        Task<IEnumerable<Employee>> GetEmployeesWithDepartmentAsync();
        Task<Employee?> GetByIdWithDepartmentAsync(int id);
        Task<bool> EmailExistsAsync(string email, int? excludeId = null);
    }
}