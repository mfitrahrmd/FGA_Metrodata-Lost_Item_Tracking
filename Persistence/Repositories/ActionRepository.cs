#region

using Application.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Action = Domain.Entities.Action;

#endregion

namespace Persistence.Repositories;

public class ActionRepository : BaseRepository<Action, ApplicationDbContext>, IActionRepository
{
    public ActionRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Action> FindOrCreateActionAsync(string name)
    {
        var foundAction = await _set.FirstOrDefaultAsync(a => a.Name.Equals(name));

        if (foundAction is null)
        {
            var newAction = new Action
            {
                Name = name
            };
            
            var res = await _set.AddAsync(newAction);

            return newAction;
        }

        return foundAction;
    }
}