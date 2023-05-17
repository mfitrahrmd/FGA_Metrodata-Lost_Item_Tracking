using Application.DAOs.Account;
using Application.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories;

public class AccountRepository : BaseRepository<Account, ApplicationDbContext>, IAccountRepository
{
    public AccountRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<RolesOfAnAccount> GetRolesOfAnAccountByEmployeeId(Guid id)
    {
        return await _set.Where(a => a.EmployeeId.Equals(id)).Select(a => new RolesOfAnAccount{Roles = a.AccountRoles.Select(ar => ar.Role.Name).ToList()}).FirstOrDefaultAsync();
    }
}