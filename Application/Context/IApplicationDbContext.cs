using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Action = Domain.Entities.Action;

namespace Application.Context;

public interface IApplicationDbContext
{
    DbSet<Employee> Employees { get; set; }
    DbSet<Item> Items { get; set; }
    DbSet<Action> Actions { get; set; }
    DbSet<ItemActions> ItemActions { get; set; }
}