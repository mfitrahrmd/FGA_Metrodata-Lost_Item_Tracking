#region

using Domain.Entities;

#endregion

namespace Application.Repositories;

public interface IRoleRepository : IBaseRepository<Role>
{
    Task<Role> FindOrCreateRoleAsync(string name);
}