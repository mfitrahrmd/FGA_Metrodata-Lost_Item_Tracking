#region

using Application.DAOs.Item;
using Application.Repositories;
using Application.Services.Common;
using Domain.Entities;
using Persistence.Context;

#endregion

namespace Persistence.Repositories;

public class ItemRepository : BaseRepository<Item, ApplicationDbContext>, IItemRepository
{
    private readonly IItemActionsRepository _itemActionsRepository;

    public ItemRepository(ApplicationDbContext context, IItemActionsRepository itemActionsRepository) : base(context)
    {
        _itemActionsRepository = itemActionsRepository;
    }

    public async Task<IQueryable<ApprovedFoundItem>> FindAllApprovedFoundItems()
    {
        var result = from i in _context.Items
            join ia in _context.ItemActions on i.Id equals ia.ItemId
            join a in _context.Actions on ia.ActionId equals a.Id
            join e in _context.Employees on ia.EmployeeId equals e.Id
            join s in _context.Status on ia.Id equals s.ItemActionsId 
            where a.Name.Equals(ActionType.Found.ToString()) where s.Name.Equals(StatusType.Approved.ToString())
            select new ApprovedFoundItem
            {
                Id = i.Id,
                Name = i.Name,
                Description = i.Description,
                ImagePath = i.ImagePath,
                FoundAt = ia.Time,
                FoundBy = e
            };

        return result;
    }
    
    public async Task<IQueryable<PendingFoundItem>> FindAllPendingFoundItems()
    {
        var result = from i in _context.Items
            join ia in _context.ItemActions on i.Id equals ia.ItemId
            join a in _context.Actions on ia.ActionId equals a.Id
            join e in _context.Employees on ia.EmployeeId equals e.Id
            join s in _context.Status on ia.Id equals s.ItemActionsId 
            where a.Name.Equals(ActionType.Found.ToString()) where s.Name.Equals(StatusType.Pending.ToString())
            select new PendingFoundItem
            {
                Id = i.Id,
                Name = i.Name,
                Description = i.Description,
                ImagePath = i.ImagePath,
                FoundAt = ia.Time,
                FoundBy = e
            };

        return result;
    }

    public async Task<IQueryable<PendingFoundItemRequestClaim>> FindAllPendingFoundItemRequestClaims()
    {
        var result = from i in _context.Items
            join ia in _context.ItemActions on i.Id equals ia.ItemId
            join a in _context.Actions on ia.ActionId equals a.Id
            join e in _context.Employees on ia.EmployeeId equals e.Id
            join s in _context.Status on ia.Id equals s.ItemActionsId 
            where a.Name.Equals(ActionType.RequestClaim.ToString()) where s.Name.Equals(StatusType.Pending.ToString())
            select new PendingFoundItemRequestClaim()
            {
                Id = i.Id,
                Name = i.Name,
                Description = i.Description,
                ImagePath = i.ImagePath,
                RequestAt = ia.Time,
                RequestBy = e
            };

        return result;
    }
}