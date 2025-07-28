using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Demo.PL.Helpers;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Demo.PL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/employee
        [HttpGet]
        public ActionResult<IEnumerable<EmployeeDto>> GetEmployees([FromQuery] string searchInp = null)
        {
            var employees = Enumerable.Empty<Employee>();

            if (string.IsNullOrEmpty(searchInp))
                employees = _unitOfWork.EmployeeRepository.GetAll();
            else
                employees = _unitOfWork.EmployeeRepository.SearchByName(searchInp.ToLower());

            var employeeDtos = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeDto>>(employees);
            return Ok(employeeDtos);
        }

        // GET: api/employee/5
        [HttpGet("{id}")]
        public ActionResult<EmployeeDto> GetEmployee(int id)
        {
            var employee = _unitOfWork.EmployeeRepository.Get(id);
            if (employee == null)
            {
                return NotFound();
            }

            var employeeDto = _mapper.Map<Employee, EmployeeDto>(employee);
            return Ok(employeeDto);
        }

        // POST: api/employee
        [HttpPost]
        public ActionResult<EmployeeDto> CreateEmployee(CreateEmployeeDto createEmployeeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = _mapper.Map<CreateEmployeeDto, Employee>(createEmployeeDto);
            employee.CreationDate = DateTime.Now;
            employee.IsDeleted = false;

            _unitOfWork.EmployeeRepository.Add(employee);
            _unitOfWork.Complete();

            var employeeDto = _mapper.Map<Employee, EmployeeDto>(employee);
            return CreatedAtAction(nameof(GetEmployee), new { id = employeeDto.Id }, employeeDto);
        }

        // PUT: api/employee/5
        [HttpPut("{id}")]
        public IActionResult UpdateEmployee(int id, UpdateEmployeeDto updateEmployeeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingEmployee = _unitOfWork.EmployeeRepository.Get(id);
            if (existingEmployee == null)
            {
                return NotFound();
            }

            // Update properties
            existingEmployee.Name = updateEmployeeDto.Name;
            existingEmployee.Age = updateEmployeeDto.Age;
            existingEmployee.Address = updateEmployeeDto.Address;
            existingEmployee.Salary = updateEmployeeDto.Salary;
            existingEmployee.IsActive = updateEmployeeDto.IsActive;
            existingEmployee.Email = updateEmployeeDto.Email;
            existingEmployee.PhoneNumber = updateEmployeeDto.PhoneNumber;
            existingEmployee.HiringDate = updateEmployeeDto.HiringDate;
            existingEmployee.DepartmentId = updateEmployeeDto.DepartmentId;

            try
            {
                _unitOfWork.EmployeeRepository.Update(existingEmployee);
                _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        // DELETE: api/employee/5
        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            var employee = _unitOfWork.EmployeeRepository.Get(id);
            if (employee == null)
            {
                return NotFound();
            }

            try
            {
                _unitOfWork.EmployeeRepository.Delete(employee);
                _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        // GET: api/employee/department/5
        [HttpGet("department/{departmentId}")]
        public ActionResult<IEnumerable<EmployeeDto>> GetEmployeesByDepartment(int departmentId)
        {
            var employees = _unitOfWork.EmployeeRepository.GetAll()
                .Where(e => e.DepartmentId == departmentId && !e.IsDeleted);

            var employeeDtos = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeDto>>(employees);
            return Ok(employeeDtos);
        }
    }
}
