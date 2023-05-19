using Application.DAOs.Item;
using Application.DTOs.Item;
using Application.Exceptions;
using Application.Repositories;
using Application.Services.Common;
using Microsoft.Extensions.Configuration;

namespace Application.Services.Item;

public class ItemService
{
    private readonly IConfiguration _configuration;
    private readonly IItemRepository _itemRepository;
    private readonly IActionRepository _actionRepository;
    private readonly IItemActionsRepository _itemActionsRepository;
    private string ItemPhotosPath { get; init; }

    public ItemService(IConfiguration configuration, IItemRepository itemRepository, IActionRepository actionRepository,
        IItemActionsRepository itemActionsRepository)
    {
        _configuration = configuration;
        _itemRepository = itemRepository;
        _actionRepository = actionRepository;
        _itemActionsRepository = itemActionsRepository;

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
            ImagePath = fileName,
            ItemActions = new List<Domain.Entities.ItemActions>
            {
                new()
                {
                    EmployeeId = employeeId,
                    Time = DateTime.Now,
                    Action = await _actionRepository.FindOrCreateActionAsync(ActionType.Found.ToString())
                }
            }
        };

        await _itemRepository.InsertOneAsync(newItem);

        return newItem;
    }

    public async Task<ICollection<ApprovedFoundItem>> FindAllApprovedFoundItems()
    {
        var approvedFoundItems = await _itemRepository.FindAllApprovedFoundItems();

        return approvedFoundItems.ToList();
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