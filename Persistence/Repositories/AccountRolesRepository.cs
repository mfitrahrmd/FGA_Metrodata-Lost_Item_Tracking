using Application.Repositories;
using Domain.Entities;
using Persistence.Context;

namespace Persistence.Repositories;

public class AccountRolesRepository : BaseRepository<AccountRoles, ApplicationDbContext>, IAccountRolesRepository
{
    public AccountRolesRepository(ApplicationDbContext context) : base(context)
    {
    }
}