using Application.DAOs.Item;
using Application.DTOs.Item;
using Application.Services.Common;
using Domain.Entities;

namespace Application.Repositories;

public interface IItemActionsRepository : IBaseRepository<ItemActions>
{
    Task<ItemActions?> FindOneByItemIdAndActionName(Guid itemId, string actionName);
    Task<ICollection<ItemActions>> FindAllByActionNameAndEmployeeId(ActionType actionType, Guid employeeId, ActionRequestQuery query);
}