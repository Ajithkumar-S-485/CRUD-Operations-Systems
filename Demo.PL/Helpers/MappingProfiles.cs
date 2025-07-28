using AutoMapper;
using Demo.DAL.Data.Migrations;
using Demo.DAL.Models;
using Demo.PL.ViewModels;

namespace Demo.PL.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // Employee mappings
            CreateMap<EmployeeViewModel, Employee>()
                .ReverseMap();
            
            CreateMap<Employee, EmployeeDto>()
                .ReverseMap();
            
            CreateMap<CreateEmployeeDto, Employee>();
            CreateMap<UpdateEmployeeDto, Employee>();
            
            // Department mappings
            CreateMap<Department, DepartmentDto>()
                .ReverseMap();
            
            CreateMap<CreateDepartmentDto, Department>();
            CreateMap<UpdateDepartmentDto, Department>();
        }
    }
}
