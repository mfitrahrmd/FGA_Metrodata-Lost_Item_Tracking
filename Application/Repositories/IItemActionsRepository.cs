using Domain.Entities;

namespace Application.Repositories;

public interface IItemActionsRepository : IBaseRepository<ItemActions>
{
    Task<ItemActions?> FindOneByItemIdAndActionName(Guid itemId, string actionName);
}