using Application.Exceptions;
using Application.Repositories;
using Application.Services.Common;
using Domain.Entities;

namespace Application.Services.ItemActions;

public class ItemActionsService
{
    private readonly IItemActionsRepository _itemActionsRepository;
    private readonly IItemRepository _itemRepository;
    private readonly IActionRepository _actionRepository;
    private readonly IStatusRepository _statusRepository;
    private readonly Status _defaultStatus = new Status
    {
        Name = StatusType.Pending.ToString(),
        Message = "your request is still being reviewed by the admin."
    };

    public ItemActionsService(IItemActionsRepository itemActionsRepository, IItemRepository itemRepository, IActionRepository actionRepository, IStatusRepository statusRepository)
    {
        _itemActionsRepository = itemActionsRepository;
        _itemRepository = itemRepository;
        _actionRepository = actionRepository;
        _statusRepository = statusRepository;
    }
    
    public async Task<Domain.Entities.ItemActions> ApproveItemAction(Guid requestId, string message)
    {
        var foundItemAction = await _itemActionsRepository.FindOneByIdAsync(requestId);

        if (foundItemAction is null)
            throw new ServiceException(ErrorType.ResourceNotFound, "Item action was not found");

        var updatedStatus = await _statusRepository.UpdateOneAsync(new Status
        {
            ItemActionsId = foundItemAction.Id,
            Name = StatusType.Approved.ToString(),
            Message = message
        });
        
        await _itemActionsRepository.UpdateOneAsync(foundItemAction);

        return foundItemAction;
    }
    
    public async Task<Domain.Entities.Item> RejectItemAction(Guid itemId, ActionType action, string message)
    {
        await IsItemExistAsync(itemId);
        
        var foundItem = await _itemRepository.FindOneByIdAsync(itemId);

        var foundItemAction = await _itemActionsRepository.FindOneByItemIdAndActionName(foundItem.Id, action.ToString());

        if (foundItemAction is null)
            throw new ServiceException(ErrorType.ResourceNotFound, "Item action was not found");

        var updatedStatus = await _statusRepository.UpdateOneAsync(new Status
        {
            ItemActionsId = foundItemAction.Id,
            Name = StatusType.Rejected.ToString(),
            Message = message
        });
        
        await _itemActionsRepository.UpdateOneAsync(foundItemAction);

        return foundItem;
    }

    public async Task<Domain.Entities.Item> AddItemAction(Guid itemId, Guid employeeId, ActionType actionType)
    {
        await IsItemExistAsync(itemId);
        
        var foundItem = await _itemRepository.FindOneByIdAsync(itemId);

        var foundOrCreatedAction = await _actionRepository.FindOrCreateActionAsync(actionType.ToString());

        var newItemActions = new Domain.Entities.ItemActions
        {
            Time = DateTime.Now,
            ItemId = foundItem.Id,
            ActionId = foundOrCreatedAction.Id,
            EmployeeId = employeeId,
            Status = new[] {ActionType.Claimed, ActionType.Found}.Contains(actionType) ? new Status{Name = "Approved", Message = "Approved"} : _defaultStatus,
        };

        await _itemActionsRepository.InsertOneAsync(newItemActions);

        return foundItem;
    }
    
    private async Task IsItemExistAsync(Guid itemId)
    {
        if (!await _itemRepository.IsExistAsync(itemId))
            throw new ServiceException(ErrorType.ResourceNotFound, "Item was not found with given Id.");
    }
}