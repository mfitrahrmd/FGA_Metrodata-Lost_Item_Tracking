#region

using Application.DAOs.Item;
using Application.DTOs.Item;
using Domain.Entities;

#endregion

namespace Application.Repositories;

public interface IItemRepository : IBaseRepository<Item>
{
    Task<ICollection<ApprovedFoundItem>> FindAllFoundItems();
    Task<ICollection<RequestFoundItem>> FindAllRequestFoundItems(ActionRequestQuery query);
    Task<ICollection<FoundItemRequestClaim>> FindAllFoundItemRequestClaims(ActionRequestQuery query);
}