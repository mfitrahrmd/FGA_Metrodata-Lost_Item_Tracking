using Application.Repositories;
using Persistence.Context;
using Action = Domain.Entities.Action;

namespace Persistence.Repositories;

public class ActionRepository : BaseRepository<Action, ApplicationDbContext>, IActionRepository
{
    public ActionRepository(ApplicationDbContext context) : base(context)
    {
    }
}