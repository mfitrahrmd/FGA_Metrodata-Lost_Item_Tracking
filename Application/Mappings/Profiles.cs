using Application.DTOs.Employee;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class Profiles : Profile
{
    public Profiles()
    {
        CreateMap<InsertOneEmployeeRequest, Employee>();
        CreateMap<Employee, EmployeeDTO>();
    }
}