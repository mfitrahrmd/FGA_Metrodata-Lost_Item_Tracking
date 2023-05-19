using Application.Exceptions;
using Application.Repositories;
using Application.Services.Common;

namespace Application.Services.ItemActions;

public class ItemActionsService
{
    private readonly IItemActionsRepository _itemActionsRepository;
    private readonly IItemRepository _itemRepository;
    private readonly IActionRepository _actionRepository;

    public ItemActionsService(IItemActionsRepository itemActionsRepository, IItemRepository itemRepository, IActionRepository actionRepository)
    {
        _itemActionsRepository = itemActionsRepository;
        _itemRepository = itemRepository;
        _actionRepository = actionRepository;
    }
    
    public async Task<Domain.Entities.ItemActions> ApproveItemAction(Guid itemId, ActionType action)
    {
        await IsItemExistAsync(itemId);
        
        var foundItem = await _itemRepository.FindOneByIdAsync(itemId);

        var foundItemAction = await _itemActionsRepository.FindOneByItemIdAndActionName(foundItem.Id, action.ToString());

        if (foundItemAction is null)
            throw new ServiceException(ErrorType.ResourceNotFound, "Item action was not found");

        foundItemAction.IsApproved = true;
        
        await _itemActionsRepository.UpdateOneAsync(foundItemAction);

        return foundItemAction;
    }

    public async Task<Domain.Entities.ItemActions> AddItemAction(Guid itemId, Guid employeeId, ActionType actionType)
    {
        await IsItemExistAsync(itemId);
        
        var foundItem = await _itemRepository.FindOneByIdAsync(itemId);

        var newItemActions = new Domain.Entities.ItemActions
        {
            IsApproved = actionType.Equals(ActionType.Claimed),
            Time = DateTime.Now,
            Action = await _actionRepository.FindOrCreateActionAsync(actionType.ToString()),
            ItemId = foundItem.Id,
            EmployeeId = employeeId,
        };

        await _itemActionsRepository.InsertOneAsync(newItemActions);

        return newItemActions;
    }
    
    private async Task IsItemExistAsync(Guid itemId)
    {
        if (!await _itemRepository.IsExistAsync(itemId))
            throw new ServiceException(ErrorType.ResourceNotFound, "Item was not found with given Id.");
    }
}