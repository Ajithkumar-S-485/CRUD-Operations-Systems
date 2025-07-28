using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Demo.PL.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesApiController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public EmployeesApiController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/EmployeesApi
        [HttpGet]
        public ActionResult<IEnumerable<EmployeeViewModel>> GetEmployees([FromQuery] string search = null)
        {
            try
            {
                var employees = string.IsNullOrEmpty(search) 
                    ? _unitOfWork.EmployeeRepository.GetAll()
                    : _unitOfWork.EmployeeRepository.SearchByName(search.ToLower());

                var mappedEmployees = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
                return Ok(mappedEmployees);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving employees", error = ex.Message });
            }
        }

        // GET: api/EmployeesApi/5
        [HttpGet("{id}")]
        public ActionResult<EmployeeViewModel> GetEmployee(int id)
        {
            try
            {
                var employee = _unitOfWork.EmployeeRepository.Get(id);

                if (employee == null)
                {
                    return NotFound(new { message = "Employee not found" });
                }

                var mappedEmployee = _mapper.Map<Employee, EmployeeViewModel>(employee);
                return Ok(mappedEmployee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the employee", error = ex.Message });
            }
        }

        // GET: api/EmployeesApi/department/5
        [HttpGet("department/{departmentId}")]
        public ActionResult<IEnumerable<EmployeeViewModel>> GetEmployeesByDepartment(int departmentId)
        {
            try
            {
                var employees = _unitOfWork.EmployeeRepository.GetAll()
                    .Where(e => e.DepartmentId == departmentId && !e.IsDeleted);

                var mappedEmployees = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
                return Ok(mappedEmployees);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving employees by department", error = ex.Message });
            }
        }

        // POST: api/EmployeesApi
        [HttpPost]
        public ActionResult<EmployeeViewModel> CreateEmployee([FromBody] EmployeeViewModel employeeViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var employee = _mapper.Map<EmployeeViewModel, Employee>(employeeViewModel);
                employee.CreationDate = DateTime.Now;
                employee.IsDeleted = false;

                _unitOfWork.EmployeeRepository.Add(employee);
                _unitOfWork.Complete();

                var createdEmployee = _mapper.Map<Employee, EmployeeViewModel>(employee);
                return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, createdEmployee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the employee", error = ex.Message });
            }
        }

        // PUT: api/EmployeesApi/5
        [HttpPut("{id}")]
        public IActionResult UpdateEmployee(int id, [FromBody] EmployeeViewModel employeeViewModel)
        {
            try
            {
                if (id != employeeViewModel.Id)
                {
                    return BadRequest(new { message = "ID mismatch" });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existingEmployee = _unitOfWork.EmployeeRepository.Get(id);
                if (existingEmployee == null)
                {
                    return NotFound(new { message = "Employee not found" });
                }

                var employee = _mapper.Map<EmployeeViewModel, Employee>(employeeViewModel);
                employee.CreationDate = existingEmployee.CreationDate; // Preserve original creation date

                _unitOfWork.EmployeeRepository.Update(employee);
                _unitOfWork.Complete();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the employee", error = ex.Message });
            }
        }

        // DELETE: api/EmployeesApi/5
        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            try
            {
                var employee = _unitOfWork.EmployeeRepository.Get(id);
                if (employee == null)
                {
                    return NotFound(new { message = "Employee not found" });
                }

                // Soft delete
                employee.IsDeleted = true;
                _unitOfWork.EmployeeRepository.Update(employee);
                _unitOfWork.Complete();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the employee", error = ex.Message });
            }
        }

        // GET: api/EmployeesApi/statistics
        [HttpGet("statistics")]
        public ActionResult<object> GetEmployeeStatistics()
        {
            try
            {
                var employees = _unitOfWork.EmployeeRepository.GetAll().Where(e => !e.IsDeleted);

                var statistics = new
                {
                    TotalEmployees = employees.Count(),
                    ActiveEmployees = employees.Count(e => e.IsActive),
                    AverageSalary = employees.Any() ? employees.Average(e => e.Salary) : 0,
                    MaxSalary = employees.Any() ? employees.Max(e => e.Salary) : 0,
                    MinSalary = employees.Any() ? employees.Min(e => e.Salary) : 0,
                    AverageAge = employees.Any() ? employees.Average(e => e.Age ?? 0) : 0
                };

                return Ok(statistics);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving statistics", error = ex.Message });
            }
        }
    }
}