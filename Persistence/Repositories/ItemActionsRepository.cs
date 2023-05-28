using Application.DTOs.Item;
using Application.Repositories;
using Application.Services.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories;

public class ItemActionsRepository : BaseRepository<ItemActions, ApplicationDbContext>, IItemActionsRepository
{
    public ItemActionsRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<ItemActions?> FindOneByItemIdAndActionName(Guid itemId, string actionName)
    {
        return await _set.FirstOrDefaultAsync(ia => ia.ItemId.Equals(itemId) && ia.Action.Name.Equals(actionName));
    }

    public async Task<ICollection<ItemActions>> FindAllByActionNameAndEmployeeId(ActionType actionType, Guid employeeId, ActionRequestQuery query)
    {
        var result = _set
            .Include(ia => ia.Item)
            .Include(ia => ia.Action)
            .Include(ia => ia.Employee)
            .Include(ia => ia.Status)
            .Where(ia => ia.EmployeeId.Equals(employeeId) && ia.Action.Name.Equals(actionType.ToString()) && (query.Status == null || ia.Status.Name.Equals(query.Status)));

        return await result.ToListAsync();
    }
}