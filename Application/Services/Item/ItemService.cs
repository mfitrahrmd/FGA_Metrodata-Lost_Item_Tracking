using Application.DTOs.Item;
using Application.Exceptions;
using Application.Repositories;
using Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace Application.Services.Item;

public class ItemService
{
    private readonly IConfiguration _configuration;
    private readonly IItemRepository _itemRepository;
    private readonly IActionRepository _actionRepository;
    private readonly IItemActionsRepository _itemActionsRepository;
    private enum Action
    {
        Found,
        Claimed
    }
    private string ItemPhotosPath { get; init; }

    public ItemService(IConfiguration configuration, IItemRepository itemRepository, IActionRepository actionRepository, IItemActionsRepository itemActionsRepository)
    {
        _configuration = configuration;
        _itemRepository = itemRepository;
        _actionRepository = actionRepository;
        _itemActionsRepository = itemActionsRepository;

        ItemPhotosPath = _configuration.GetValue<string>("Application:ItemPhotosPath");

        // check item photos path in appsettings
        if (ItemPhotosPath.Equals(""))
            throw new ApplicationException("Item photos path is required.");

        // initialize folder to store item photos
        Directory.CreateDirectory(ItemPhotosPath);
    }
    
    public async Task<Domain.Entities.Item> ReportFoundItem(InsertFoundItemRequest request, UserIdentity userIdentity)
    {
        var fileName = await SaveImageAsync(request, userIdentity);

        var newItem = new Domain.Entities.Item
        {
            Name = request.Name,
            Description = request.Description,
            EmployeeId = userIdentity.Id,
            ImagePath = fileName,
            ItemActions = new List<ItemActions>
            {
                new()
                {
                    Time = DateTime.Now,
                    Action = await _actionRepository.FindOrCreateActionAsync(Action.Found.ToString())
                }
            }
        };
        
        await _itemRepository.InsertOneAsync(newItem);

        return newItem;
    }

    public async Task<Domain.Entities.ItemActions> ApproveFoundItem(Guid itemId)
    {
        if (!await _itemRepository.IsExistAsync(itemId))
            throw new ServiceException(ErrorType.ResourceNotFound, "Item was not found with given Id.");
        
        var foundItem = await _itemRepository.FindOneByIdAsync(itemId);

        var foundItemAction = await _itemActionsRepository.FindOneByItemIdAndActionName(foundItem.Id, Action.Found.ToString());

        if (foundItemAction is null)
            throw new ServiceException(ErrorType.ResourceNotFound, "Item action was not found");

        foundItemAction.IsApproved = true;
        
        await _itemActionsRepository.UpdateOneAsync(foundItemAction);

        await _itemRepository.UpdateOneAsync(foundItem);

        return foundItemAction;
    }

    private async Task<string> SaveImageAsync(InsertFoundItemRequest request, UserIdentity userIdentity)
    {
        string fileName;
        
        // simply return empty string if there's no file
        if (request.File is null)
            return "";
        
        // file name format : guid~user id who found it~item_found
        fileName = $"{Guid.NewGuid()}~{userIdentity.Id}~item_found~{request.Name}{Path.GetExtension(request.File.FileName)}".Replace(" ", "_");

        using (var fileStream = new FileStream(Path.Combine(ItemPhotosPath, fileName), FileMode.CreateNew))
        {
            try
            {
                await request.File.CopyToAsync(fileStream);
            
                fileStream.Close();
            }
            catch (Exception e)
            {
                throw new ApplicationException($"Failed to save item photo : {e.Message}");
            }
        }

        return fileName;
    }
}

public struct UserIdentity
{
    public Guid Id { get; }
    public string Nik { get; }
    public IList<string> Roles { get; }

    public UserIdentity(Guid id, string nik, IList<string> roles)
    {
        Id = id;
        Nik = nik;
        Roles = roles;
    }
}