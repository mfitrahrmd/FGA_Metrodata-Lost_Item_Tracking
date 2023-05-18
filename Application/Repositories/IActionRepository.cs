#region

using Action = Domain.Entities.Action;

#endregion

namespace Application.Repositories;

public interface IActionRepository : IBaseRepository<Action>
{
}