using Demo.WebApi.DTOs;
using Demo.WebApi.Interfaces;
using Demo.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Demo.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployees()
        {
            var employees = await _unitOfWork.Employees.GetAllAsync();
            var employeeDtos = employees.Select(e => new EmployeeDto
            {
                Id = e.Id,
                Name = e.Name,
                Age = e.Age,
                Address = e.Address,
                Salary = e.Salary,
                IsActive = e.IsActive,
                Email = e.Email,
                PhoneNumber = e.PhoneNumber,
                HiringDate = e.HiringDate,
                CreationDate = e.CreationDate,
                ImageName = e.ImageName,
                DepartmentId = e.DepartmentId,
                DepartmentName = e.Department?.Name ?? ""
            });

            return Ok(employeeDtos);
        }

        // GET: api/employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDto>> GetEmployee(int id)
        {
            var employee = await _unitOfWork.Employees.GetByIdWithDepartmentAsync(id);
            if (employee == null)
            {
                return NotFound($"Employee with ID {id} not found.");
            }

            var employeeDto = new EmployeeDto
            {
                Id = employee.Id,
                Name = employee.Name,
                Age = employee.Age,
                Address = employee.Address,
                Salary = employee.Salary,
                IsActive = employee.IsActive,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                HiringDate = employee.HiringDate,
                CreationDate = employee.CreationDate,
                ImageName = employee.ImageName,
                DepartmentId = employee.DepartmentId,
                DepartmentName = employee.Department?.Name ?? ""
            };

            return Ok(employeeDto);
        }

        // GET: api/employees/active
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetActiveEmployees()
        {
            var employees = await _unitOfWork.Employees.GetActiveEmployeesAsync();
            var employeeDtos = employees.Select(e => new EmployeeDto
            {
                Id = e.Id,
                Name = e.Name,
                Age = e.Age,
                Address = e.Address,
                Salary = e.Salary,
                IsActive = e.IsActive,
                Email = e.Email,
                PhoneNumber = e.PhoneNumber,
                HiringDate = e.HiringDate,
                CreationDate = e.CreationDate,
                ImageName = e.ImageName,
                DepartmentId = e.DepartmentId,
                DepartmentName = e.Department?.Name ?? ""
            });

            return Ok(employeeDtos);
        }

        // POST: api/employees
        [HttpPost]
        public async Task<ActionResult<EmployeeDto>> CreateEmployee(CreateEmployeeDto createEmployeeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if email already exists
            if (!string.IsNullOrEmpty(createEmployeeDto.Email) &&
                await _unitOfWork.Employees.EmailExistsAsync(createEmployeeDto.Email))
            {
                return Conflict($"Employee with email '{createEmployeeDto.Email}' already exists.");
            }

            // Validate department exists if provided
            if (createEmployeeDto.DepartmentId.HasValue)
            {
                var departmentExists = await _unitOfWork.Departments.ExistsAsync(createEmployeeDto.DepartmentId.Value);
                if (!departmentExists)
                {
                    return BadRequest($"Department with ID {createEmployeeDto.DepartmentId} does not exist.");
                }
            }

            var employee = new Employee
            {
                Name = createEmployeeDto.Name,
                Age = createEmployeeDto.Age,
                Address = createEmployeeDto.Address,
                Salary = createEmployeeDto.Salary,
                IsActive = createEmployeeDto.IsActive,
                Email = createEmployeeDto.Email,
                PhoneNumber = createEmployeeDto.PhoneNumber,
                HiringDate = createEmployeeDto.HiringDate,
                ImageName = createEmployeeDto.ImageName,
                DepartmentId = createEmployeeDto.DepartmentId,
                CreationDate = DateTime.Now,
                IsDeleted = false
            };

            await _unitOfWork.Employees.AddAsync(employee);
            await _unitOfWork.CompleteAsync();

            // Get the created employee with department info
            var createdEmployee = await _unitOfWork.Employees.GetByIdWithDepartmentAsync(employee.Id);
            var employeeDto = new EmployeeDto
            {
                Id = createdEmployee!.Id,
                Name = createdEmployee.Name,
                Age = createdEmployee.Age,
                Address = createdEmployee.Address,
                Salary = createdEmployee.Salary,
                IsActive = createdEmployee.IsActive,
                Email = createdEmployee.Email,
                PhoneNumber = createdEmployee.PhoneNumber,
                HiringDate = createdEmployee.HiringDate,
                CreationDate = createdEmployee.CreationDate,
                ImageName = createdEmployee.ImageName,
                DepartmentId = createdEmployee.DepartmentId,
                DepartmentName = createdEmployee.Department?.Name ?? ""
            };

            return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employeeDto);
        }

        // PUT: api/employees/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, UpdateEmployeeDto updateEmployeeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = await _unitOfWork.Employees.GetByIdAsync(id);
            if (employee == null)
            {
                return NotFound($"Employee with ID {id} not found.");
            }

            // Check if email already exists (excluding current employee)
            if (!string.IsNullOrEmpty(updateEmployeeDto.Email) &&
                await _unitOfWork.Employees.EmailExistsAsync(updateEmployeeDto.Email, id))
            {
                return Conflict($"Employee with email '{updateEmployeeDto.Email}' already exists.");
            }

            // Validate department exists if provided
            if (updateEmployeeDto.DepartmentId.HasValue)
            {
                var departmentExists = await _unitOfWork.Departments.ExistsAsync(updateEmployeeDto.DepartmentId.Value);
                if (!departmentExists)
                {
                    return BadRequest($"Department with ID {updateEmployeeDto.DepartmentId} does not exist.");
                }
            }

            employee.Name = updateEmployeeDto.Name;
            employee.Age = updateEmployeeDto.Age;
            employee.Address = updateEmployeeDto.Address;
            employee.Salary = updateEmployeeDto.Salary;
            employee.IsActive = updateEmployeeDto.IsActive;
            employee.Email = updateEmployeeDto.Email;
            employee.PhoneNumber = updateEmployeeDto.PhoneNumber;
            employee.HiringDate = updateEmployeeDto.HiringDate;
            employee.ImageName = updateEmployeeDto.ImageName;
            employee.DepartmentId = updateEmployeeDto.DepartmentId;

            await _unitOfWork.Employees.UpdateAsync(employee);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        // DELETE: api/employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _unitOfWork.Employees.GetByIdAsync(id);
            if (employee == null)
            {
                return NotFound($"Employee with ID {id} not found.");
            }

            await _unitOfWork.Employees.DeleteAsync(id); // This will soft delete (set IsDeleted = true)
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        // GET: api/employees/department/5
        [HttpGet("department/{departmentId}")]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployeesByDepartment(int departmentId)
        {
            var departmentExists = await _unitOfWork.Departments.ExistsAsync(departmentId);
            if (!departmentExists)
            {
                return NotFound($"Department with ID {departmentId} not found.");
            }

            var employees = await _unitOfWork.Employees.GetEmployeesByDepartmentAsync(departmentId);
            var employeeDtos = employees.Select(e => new EmployeeDto
            {
                Id = e.Id,
                Name = e.Name,
                Age = e.Age,
                Address = e.Address,
                Salary = e.Salary,
                IsActive = e.IsActive,
                Email = e.Email,
                PhoneNumber = e.PhoneNumber,
                HiringDate = e.HiringDate,
                CreationDate = e.CreationDate,
                ImageName = e.ImageName,
                DepartmentId = e.DepartmentId,
                DepartmentName = e.Department?.Name ?? ""
            });

            return Ok(employeeDtos);
        }
    }
}