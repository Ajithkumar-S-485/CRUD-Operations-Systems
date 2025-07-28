using Demo.WebApi.DTOs;
using Demo.WebApi.Interfaces;
using Demo.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Demo.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/departments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetDepartments()
        {
            var departments = await _unitOfWork.Departments.GetAllAsync();
            var departmentDtos = departments.Select(d => new DepartmentDto
            {
                Id = d.Id,
                Code = d.Code,
                Name = d.Name,
                DateOfCreation = d.DateOfCreation,
                EmployeeCount = d.Employees.Count
            });

            return Ok(departmentDtos);
        }

        // GET: api/departments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DepartmentDto>> GetDepartment(int id)
        {
            var department = await _unitOfWork.Departments.GetByIdAsync(id);
            if (department == null)
            {
                return NotFound($"Department with ID {id} not found.");
            }

            var departmentDto = new DepartmentDto
            {
                Id = department.Id,
                Code = department.Code,
                Name = department.Name,
                DateOfCreation = department.DateOfCreation,
                EmployeeCount = department.Employees.Count
            };

            return Ok(departmentDto);
        }

        // GET: api/departments/by-code/IT
        [HttpGet("by-code/{code}")]
        public async Task<ActionResult<DepartmentDto>> GetDepartmentByCode(string code)
        {
            var department = await _unitOfWork.Departments.GetByCodeAsync(code);
            if (department == null)
            {
                return NotFound($"Department with code '{code}' not found.");
            }

            var departmentDto = new DepartmentDto
            {
                Id = department.Id,
                Code = department.Code,
                Name = department.Name,
                DateOfCreation = department.DateOfCreation,
                EmployeeCount = department.Employees.Count
            };

            return Ok(departmentDto);
        }

        // POST: api/departments
        [HttpPost]
        public async Task<ActionResult<DepartmentDto>> CreateDepartment(CreateDepartmentDto createDepartmentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if code already exists
            if (await _unitOfWork.Departments.CodeExistsAsync(createDepartmentDto.Code))
            {
                return Conflict($"Department with code '{createDepartmentDto.Code}' already exists.");
            }

            var department = new Department
            {
                Code = createDepartmentDto.Code,
                Name = createDepartmentDto.Name,
                DateOfCreation = DateTime.Now
            };

            await _unitOfWork.Departments.AddAsync(department);
            await _unitOfWork.CompleteAsync();

            var departmentDto = new DepartmentDto
            {
                Id = department.Id,
                Code = department.Code,
                Name = department.Name,
                DateOfCreation = department.DateOfCreation,
                EmployeeCount = 0
            };

            return CreatedAtAction(nameof(GetDepartment), new { id = department.Id }, departmentDto);
        }

        // PUT: api/departments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(int id, UpdateDepartmentDto updateDepartmentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var department = await _unitOfWork.Departments.GetByIdAsync(id);
            if (department == null)
            {
                return NotFound($"Department with ID {id} not found.");
            }

            // Check if code already exists (excluding current department)
            if (await _unitOfWork.Departments.CodeExistsAsync(updateDepartmentDto.Code, id))
            {
                return Conflict($"Department with code '{updateDepartmentDto.Code}' already exists.");
            }

            department.Code = updateDepartmentDto.Code;
            department.Name = updateDepartmentDto.Name;

            await _unitOfWork.Departments.UpdateAsync(department);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        // DELETE: api/departments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var department = await _unitOfWork.Departments.GetByIdAsync(id);
            if (department == null)
            {
                return NotFound($"Department with ID {id} not found.");
            }

            // Check if department has employees
            if (department.Employees.Any())
            {
                return BadRequest("Cannot delete department that has employees. Please reassign or remove employees first.");
            }

            await _unitOfWork.Departments.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        // GET: api/departments/5/employees
        [HttpGet("{id}/employees")]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetDepartmentEmployees(int id)
        {
            var department = await _unitOfWork.Departments.GetByIdAsync(id);
            if (department == null)
            {
                return NotFound($"Department with ID {id} not found.");
            }

            var employees = await _unitOfWork.Employees.GetEmployeesByDepartmentAsync(id);
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