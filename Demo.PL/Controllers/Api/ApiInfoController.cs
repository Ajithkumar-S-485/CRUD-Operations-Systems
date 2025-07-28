using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Demo.PL.Controllers.Api
{
    [Route("api")]
    [ApiController]
    public class ApiInfoController : ControllerBase
    {
        [HttpGet]
        public ActionResult<object> GetApiInfo()
        {
            var apiInfo = new
            {
                Name = "Employee Management System API",
                Version = "1.0.0",
                Description = "A RESTful API for managing employees and departments",
                BaseUrl = $"{Request.Scheme}://{Request.Host}",
                Endpoints = new
                {
                    Employees = new
                    {
                        GetAll = "GET /api/EmployeesApi",
                        GetById = "GET /api/EmployeesApi/{id}",
                        GetByDepartment = "GET /api/EmployeesApi/department/{departmentId}",
                        Create = "POST /api/EmployeesApi",
                        Update = "PUT /api/EmployeesApi/{id}",
                        Delete = "DELETE /api/EmployeesApi/{id}",
                        Statistics = "GET /api/EmployeesApi/statistics"
                    },
                    Departments = new
                    {
                        GetAll = "GET /api/DepartmentsApi",
                        GetById = "GET /api/DepartmentsApi/{id}",
                        GetEmployees = "GET /api/DepartmentsApi/{id}/employees",
                        Create = "POST /api/DepartmentsApi",
                        Update = "PUT /api/DepartmentsApi/{id}",
                        Delete = "DELETE /api/DepartmentsApi/{id}",
                        Statistics = "GET /api/DepartmentsApi/statistics"
                    }
                },
                SampleRequests = new
                {
                    CreateEmployee = new
                    {
                        Method = "POST",
                        Url = "/api/EmployeesApi",
                        Body = new
                        {
                            Name = "John Doe",
                            Age = 30,
                            Address = "123 Main St, City",
                            Salary = 75000.00,
                            IsActive = true,
                            Email = "john.doe@company.com",
                            PhoneNumber = "555-0101",
                            HiringDate = "2023-01-15T00:00:00",
                            DepartmentId = 1
                        }
                    },
                    CreateDepartment = new
                    {
                        Method = "POST",
                        Url = "/api/DepartmentsApi",
                        Body = new
                        {
                            Code = "IT",
                            Name = "Information Technology"
                        }
                    }
                }
            };

            return Ok(apiInfo);
        }

        [HttpGet("health")]
        public ActionResult<object> GetHealthStatus()
        {
            var healthStatus = new
            {
                Status = "Healthy",
                Timestamp = DateTime.UtcNow,
                Version = "1.0.0",
                Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"
            };

            return Ok(healthStatus);
        }
    }
}