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
using Application.DTOs.Status;
using Application.DTOs.User;
using AutoMapper;
using Domain.Entities;
using Action = Domain.Entities.Action;

#endregion

namespace Application.Mappings;

public class Profiles : Profile
{
    public Profiles()
    {
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
        CreateMap<RequestFoundItem, FoundItemDTO>();
        CreateMap<Status, StatusDTO>().ForMember(sd => sd.Id, expression =>
        {
            expression.MapFrom(s => s.ItemActionsId);
        });
        CreateMap<ItemActions, MyRequestFoundResponse>();
        CreateMap<ItemActions, MyRequestClaimResponse>();
        CreateMap<Application.DTOs.Item.InsertItemRequest, Item>();
        CreateMap<Application.DTOs.Item.UpdateItemRequest, Item>();
        CreateMap<Application.DTOs.Employee.InsertEmployeeRequest, Employee>();
        CreateMap<Application.DTOs.Employee.UpdateEmployeeRequest, Employee>();
        CreateMap<Application.DTOs.Account.InsertAccountRequest, Account>();
        CreateMap<Application.DTOs.Account.UpdateAccountRequest, Account>();
        CreateMap<Application.DTOs.Department.InsertDepartmentRequest, Department>();
        CreateMap<Application.DTOs.Department.UpdateDepartmentRequest, Department>();
        CreateMap<Application.DTOs.AccountRoles.InsertAccountRolesRequest, AccountRoles>();
        CreateMap<Application.DTOs.AccountRoles.UpdateAccountRolesRequest, AccountRoles>();
        CreateMap<Application.DTOs.Role.InsertRoleRequest, Role>();
        CreateMap<Application.DTOs.Role.UpdateRoleRequest, Role>();
    }
}