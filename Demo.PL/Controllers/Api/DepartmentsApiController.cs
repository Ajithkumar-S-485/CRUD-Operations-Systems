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
    public class DepartmentsApiController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentsApiController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/DepartmentsApi
        [HttpGet]
        public ActionResult<IEnumerable<DepartmentViewModel>> GetDepartments()
        {
            try
            {
                var departments = _unitOfWork.DepartmentRepository.GetAll();
                var mappedDepartments = _mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel>>(departments);
                return Ok(mappedDepartments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving departments", error = ex.Message });
            }
        }

        // GET: api/DepartmentsApi/5
        [HttpGet("{id}")]
        public ActionResult<DepartmentViewModel> GetDepartment(int id)
        {
            try
            {
                var department = _unitOfWork.DepartmentRepository.Get(id);

                if (department == null)
                {
                    return NotFound(new { message = "Department not found" });
                }

                var mappedDepartment = _mapper.Map<Department, DepartmentViewModel>(department);
                return Ok(mappedDepartment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the department", error = ex.Message });
            }
        }

        // GET: api/DepartmentsApi/5/employees
        [HttpGet("{id}/employees")]
        public ActionResult<IEnumerable<EmployeeViewModel>> GetDepartmentEmployees(int id)
        {
            try
            {
                var department = _unitOfWork.DepartmentRepository.Get(id);
                if (department == null)
                {
                    return NotFound(new { message = "Department not found" });
                }

                var employees = department.Employees.Where(e => !e.IsDeleted);
                var mappedEmployees = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
                return Ok(mappedEmployees);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving department employees", error = ex.Message });
            }
        }

        // POST: api/DepartmentsApi
        [HttpPost]
        public ActionResult<DepartmentViewModel> CreateDepartment([FromBody] DepartmentViewModel departmentViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var department = _mapper.Map<DepartmentViewModel, Department>(departmentViewModel);
                department.DateOfCreation = DateTime.Now;
                department.CreationDate = DateTime.Now;
                department.IsDeleted = false;

                _unitOfWork.DepartmentRepository.Add(department);
                _unitOfWork.Complete();

                var createdDepartment = _mapper.Map<Department, DepartmentViewModel>(department);
                return CreatedAtAction(nameof(GetDepartment), new { id = department.Id }, createdDepartment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the department", error = ex.Message });
            }
        }

        // PUT: api/DepartmentsApi/5
        [HttpPut("{id}")]
        public IActionResult UpdateDepartment(int id, [FromBody] DepartmentViewModel departmentViewModel)
        {
            try
            {
                if (id != departmentViewModel.Id)
                {
                    return BadRequest(new { message = "ID mismatch" });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existingDepartment = _unitOfWork.DepartmentRepository.Get(id);
                if (existingDepartment == null)
                {
                    return NotFound(new { message = "Department not found" });
                }

                var department = _mapper.Map<DepartmentViewModel, Department>(departmentViewModel);
                department.DateOfCreation = existingDepartment.DateOfCreation; // Preserve original creation date
                department.CreationDate = existingDepartment.CreationDate;

                _unitOfWork.DepartmentRepository.Update(department);
                _unitOfWork.Complete();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the department", error = ex.Message });
            }
        }

        // DELETE: api/DepartmentsApi/5
        [HttpDelete("{id}")]
        public IActionResult DeleteDepartment(int id)
        {
            try
            {
                var department = _unitOfWork.DepartmentRepository.Get(id);
                if (department == null)
                {
                    return NotFound(new { message = "Department not found" });
                }

                // Check if department has employees
                if (department.Employees.Any(e => !e.IsDeleted))
                {
                    return BadRequest(new { message = "Cannot delete department with active employees" });
                }

                // Soft delete
                department.IsDeleted = true;
                _unitOfWork.DepartmentRepository.Update(department);
                _unitOfWork.Complete();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the department", error = ex.Message });
            }
        }

        // GET: api/DepartmentsApi/statistics
        [HttpGet("statistics")]
        public ActionResult<object> GetDepartmentStatistics()
        {
            try
            {
                var departments = _unitOfWork.DepartmentRepository.GetAll().Where(d => !d.IsDeleted);

                var statistics = new
                {
                    TotalDepartments = departments.Count(),
                    DepartmentsWithEmployees = departments.Count(d => d.Employees.Any(e => !e.IsDeleted)),
                    AverageEmployeesPerDepartment = departments.Any() 
                        ? departments.Average(d => d.Employees.Count(e => !e.IsDeleted)) 
                        : 0
                };

                return Ok(statistics);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving department statistics", error = ex.Message });
            }
        }
    }
}