using Application.DAOs.Item;
using Application.DTOs.Item;
using Application.Exceptions;
using Application.Repositories;
using Application.Services.Common;
using Application.Services.ItemActions;
using Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace Application.Services.Item;

public class ItemService
{
    private readonly IConfiguration _configuration;
    private readonly IItemRepository _itemRepository;
    private readonly ItemActionsService _itemActionsService;
    private string ItemPhotosPath { get; init; }

    public ItemService(IConfiguration configuration, IItemRepository itemRepository, ItemActionsService itemActionsService)
    {
        _configuration = configuration;
        _itemRepository = itemRepository;
        _itemActionsService = itemActionsService;

        ItemPhotosPath = _configuration.GetValue<string>("Application:ItemPhotosPath");

        // check item photos path in appsettings
        if (ItemPhotosPath.Equals(""))
            throw new ServiceException(ErrorType.Internal, "Item photos path is required.");

        // initialize folder to store item photos
        Directory.CreateDirectory(ItemPhotosPath);
    }

    public async Task<Domain.Entities.Item> RequestFoundItem(InsertFoundItemRequest request, Guid employeeId)
    {
        var fileName = await SaveImageAsync(request, employeeId);

        var insertedItem = await _itemRepository.InsertOneAsync(new()
        {
            Name = request.Name,
            Description = request.Description,
            ImagePath = fileName
        });

        await _itemActionsService.AddItemAction(insertedItem.Id, employeeId, ActionType.RequestFound);

        return insertedItem;
    }

    public async Task<ICollection<ApprovedFoundItem>> FindAllFoundItems()
    {
        var approvedFoundItems = await _itemRepository.FindAllFoundItems();

        return approvedFoundItems.ToList();
    }

    public async Task<ICollection<RequestFoundItem>> FindAllRequestFoundItems(ActionRequestQuery query)
    {
        var requestFoundItems = await _itemRepository.FindAllRequestFoundItems(query);

        return requestFoundItems.ToList();
    }

    public async Task<ICollection<FoundItemRequestClaim>> FindAllFoundItemRequestClaims(ActionRequestQuery query)
    {
        var foundItemRequestClaims = await _itemRepository.FindAllFoundItemRequestClaims(query);

        return foundItemRequestClaims.ToList();
    }

    private async Task<string> SaveImageAsync(InsertFoundItemRequest request, Guid employeeId)
    {
        string fileName;

        // simply return empty string if there's no file
        if (request.File is null)
            return "";

        // file name format : guid~item_found~name
        fileName =
            $"{Guid.NewGuid()}~item_found~{request.Name}{Path.GetExtension(request.File.FileName)}"
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