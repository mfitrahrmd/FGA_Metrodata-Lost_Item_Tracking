#region

using Application.DAOs.Item;
using Domain.Entities;

#endregion

namespace Application.Repositories;

public interface IItemRepository : IBaseRepository<Item>
{
    Task<IQueryable<ApprovedFoundItem>> FindAllApprovedFoundItems();
    Task<IQueryable<PendingFoundItem>> FindAllPendingFoundItems();
    Task<IQueryable<PendingFoundItemRequestClaim>> FindAllPendingFoundItemRequestClaims();
}