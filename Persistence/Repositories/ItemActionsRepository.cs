using Application.Repositories;
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
}