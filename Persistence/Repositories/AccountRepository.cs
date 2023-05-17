using Application.Repositories;
using Domain.Entities;
using Persistence.Context;

namespace Persistence.Repositories;

public class AccountRepository : BaseRepository<Account, ApplicationDbContext>, IAccountRepository
{
    public AccountRepository(ApplicationDbContext context) : base(context)
    {
    }
}