using Application.DAOs.Item;
using Application.DTOs.Item;
using Application.Exceptions;
using Application.Repositories;
using Application.Services.Common;
using Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace Application.Services.Item;

public class ItemService
{
    private readonly IConfiguration _configuration;
    private readonly IItemRepository _itemRepository;
    private readonly IActionRepository _actionRepository;
    private readonly IItemActionsRepository _itemActionsRepository;
    private readonly IStatusRepository _statusRepository;
    private string ItemPhotosPath { get; init; }
    private readonly Status _defaultStatus = new Status
    {
        Name = StatusType.Pending.ToString(),
        Message = "your request is still being reviewed by the admin."
    };

    public ItemService(IConfiguration configuration, IItemRepository itemRepository, IActionRepository actionRepository,
        IItemActionsRepository itemActionsRepository, IStatusRepository statusRepository)
    {
        _configuration = configuration;
        _itemRepository = itemRepository;
        _actionRepository = actionRepository;
        _itemActionsRepository = itemActionsRepository;
        _statusRepository = statusRepository;

        ItemPhotosPath = _configuration.GetValue<string>("Application:ItemPhotosPath");

        // check item photos path in appsettings
        if (ItemPhotosPath.Equals(""))
            throw new ServiceException(ErrorType.Internal, "Item photos path is required.");

        // initialize folder to store item photos
        Directory.CreateDirectory(ItemPhotosPath);
    }

    public async Task<Domain.Entities.Item> ReportFoundItem(InsertFoundItemRequest request, Guid employeeId)
    {
        var fileName = await SaveImageAsync(request, employeeId);

        var newItem = new Domain.Entities.Item
        {
            Name = request.Name,
            Description = request.Description,
            ImagePath = fileName
        };

        var insertedItem = await _itemRepository.InsertOneAsync(newItem);

        var foundOrCreatedAction = await _actionRepository.FindOrCreateActionAsync(ActionType.Found.ToString());

        var foundOrCreatedStatus = await _statusRepository.FindOrCreateStatusAsync(_defaultStatus);

        await _itemActionsRepository.InsertOneAsync(new Domain.Entities.ItemActions
        {
            Time = DateTime.Now,
            ItemId = insertedItem.Id,
            ActionId = foundOrCreatedAction.Id,
            EmployeeId = employeeId,
            Status = foundOrCreatedStatus
        });

        return newItem;
    }

    public async Task<ICollection<ApprovedFoundItem>> FindAllApprovedFoundItems()
    {
        var approvedFoundItems = await _itemRepository.FindAllApprovedFoundItems();

        return approvedFoundItems.ToList();
    }
    
    public async Task<ICollection<PendingFoundItem>> FindAllAPendingFoundItems()
    {
        var pendingFoundItems = await _itemRepository.FindAllPendingFoundItems();

        return pendingFoundItems.ToList();
    }
    
    public async Task<ICollection<PendingFoundItemRequestClaim>> FindAllPendingFoundItemRequestClaims()
    {
        var pendingFoundItemRequestClaims = await _itemRepository.FindAllPendingFoundItemRequestClaims();

        return pendingFoundItemRequestClaims.ToList();
    }

    private async Task<string> SaveImageAsync(InsertFoundItemRequest request, Guid employeeId)
    {
        string fileName;

        // simply return empty string if there's no file
        if (request.File is null)
            return "";

        // file name format : guid~user id who found it~item_found
        fileName =
            $"{Guid.NewGuid()}~{employeeId}~item_found~{request.Name}{Path.GetExtension(request.File.FileName)}"
                .Replace(" ", "_");

        using (var fileStream = new FileStream(Path.Combine(ItemPhotosPath, fileName), FileMode.CreateNew))
        {
            try
            {
                await request.File.CopyToAsync(fileStream);

                fileStream.Close();
            }
            catch (Exception e)
            {
                throw new ServiceException(ErrorType.Internal, $"Failed to save item photo : {e.Message}");
            }
        }

        return fileName;
    }
}