#region

using Application.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

#endregion

namespace Persistence.Repositories;

public class RoleRepository : BaseRepository<Role, ApplicationDbContext>, IRoleRepository
{
    public RoleRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Role> FindOrCreateRoleAsync(string name)
    {
        var foundRole = await _set.FirstOrDefaultAsync(r => r.Name.Equals(name));

        if (foundRole is null)
        {
            var newRole = new Role
            {
                Name = name
            };
            
            var res = await _set.AddAsync(newRole);

            return newRole;
        }

        return foundRole;
    }
}