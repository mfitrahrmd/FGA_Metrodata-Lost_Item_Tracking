using Application.DTOs.Account;
using Application.DTOs.Action;
using Application.DTOs.Department;
using Application.DTOs.Employee;
using Application.DTOs.Item;
using Application.DTOs.Role;
using AutoMapper;
using Domain.Entities;
using Action = Domain.Entities.Action;

namespace Application.Mappings;

public class Profiles : Profile
{
    public Profiles()
    {
        CreateMap<InsertOneEmployeeRequest, Employee>();
        CreateMap<Employee, EmployeeDTO>();
        CreateMap<Account, AccountDTO>();
        CreateMap<Action, ActionDTO>();
        CreateMap<Department, DepartmentDTO>();
        CreateMap<Item, ItemDTO>();
        CreateMap<Role, RoleDTO>();
        CreateMap<RegisterRequest, Employee>();
    }
}