#region

using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Action = Domain.Entities.Action;

#endregion

namespace Application.Context;

public interface IApplicationDbContext
{
    DbSet<Employee> Employees { get; set; }
    DbSet<Item> Items { get; set; }
    DbSet<Action> Actions { get; set; }
    DbSet<ItemActions> ItemActions { get; set; }
    DbSet<Department> Departments { get; set; }
    DbSet<Account> Accounts { get; set; }
    DbSet<Role> Roles { get; set; }
    DbSet<AccountRoles> AccountRoles { get; set; }
    DbSet<Status> Status { get; set; }
}