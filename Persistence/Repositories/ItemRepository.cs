#region

using Application.Repositories;
using Domain.Entities;
using Persistence.Context;

#endregion

namespace Persistence.Repositories;

public class ItemRepository : BaseRepository<Item, ApplicationDbContext>, IItemRepository
{
    public ItemRepository(ApplicationDbContext context) : base(context)
    {
    }
}