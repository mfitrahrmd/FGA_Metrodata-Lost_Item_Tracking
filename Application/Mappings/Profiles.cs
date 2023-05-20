#region

using Application.DAOs.Item;
using Application.DTOs.Account;
using Application.DTOs.AccountRoles;
using Application.DTOs.Action;
using Application.DTOs.Department;
using Application.DTOs.Employee;
using Application.DTOs.Item;
using Application.DTOs.ItemActions;
using Application.DTOs.Role;
using AutoMapper;
using Domain.Entities;
using Action = Domain.Entities.Action;

#endregion

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
        CreateMap<InsertFoundItemRequest, Item>();
        CreateMap<ItemActions, ItemActionsDTO>();
        CreateMap<AccountRoles, AccountRolesDTO>();
        CreateMap<ApprovedFoundItem, FoundItemDTO>();
        CreateMap<PendingFoundItem, FoundItemDTO>();
    }
}