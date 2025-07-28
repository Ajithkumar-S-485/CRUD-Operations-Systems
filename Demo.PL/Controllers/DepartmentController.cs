using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Demo.PL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/department
        [HttpGet]
        public ActionResult<IEnumerable<DepartmentDto>> GetDepartments()
        {
            var departments = _unitOfWork.DepartmentRepository.GetAll();
            var departmentDtos = _mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentDto>>(departments);
            return Ok(departmentDtos);
        }

        // GET: api/department/5
        [HttpGet("{id}")]
        public ActionResult<DepartmentDto> GetDepartment(int id)
        {
            var department = _unitOfWork.DepartmentRepository.Get(id);
            if (department == null)
            {
                return NotFound();
            }

            var departmentDto = _mapper.Map<Department, DepartmentDto>(department);
            return Ok(departmentDto);
        }

        // POST: api/department
        [HttpPost]
        public ActionResult<DepartmentDto> CreateDepartment(CreateDepartmentDto createDepartmentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var department = _mapper.Map<CreateDepartmentDto, Department>(createDepartmentDto);
            department.DateOfCreation = DateTime.Now;

            _unitOfWork.DepartmentRepository.Add(department);
            _unitOfWork.Complete();

            var departmentDto = _mapper.Map<Department, DepartmentDto>(department);
            return CreatedAtAction(nameof(GetDepartment), new { id = departmentDto.Id }, departmentDto);
        }

        // PUT: api/department/5
        [HttpPut("{id}")]
        public IActionResult UpdateDepartment(int id, UpdateDepartmentDto updateDepartmentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingDepartment = _unitOfWork.DepartmentRepository.Get(id);
            if (existingDepartment == null)
            {
                return NotFound();
            }

            // Update properties
            existingDepartment.Code = updateDepartmentDto.Code;
            existingDepartment.Name = updateDepartmentDto.Name;

            try
            {
                _unitOfWork.DepartmentRepository.Update(existingDepartment);
                _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        // DELETE: api/department/5
        [HttpDelete("{id}")]
        public IActionResult DeleteDepartment(int id)
        {
            var department = _unitOfWork.DepartmentRepository.Get(id);
            if (department == null)
            {
                return NotFound();
            }

            try
            {
                _unitOfWork.DepartmentRepository.Delete(department);
                _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }
    }
}
