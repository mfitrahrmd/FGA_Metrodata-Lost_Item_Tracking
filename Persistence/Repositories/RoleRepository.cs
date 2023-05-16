using Application.Repositories;
using Domain.Entities;
using Persistence.Context;

namespace Persistence.Repositories;

public class RoleRepository : BaseRepository<Role, ApplicationDbContext>, IRoleRepository
{
    public RoleRepository(ApplicationDbContext context) : base(context)
    {
    }
}