using Application.DAOs.Account;
using Domain.Entities;

namespace Application.Repositories;

public interface IAccountRepository : IBaseRepository<Account>
{
    Task<RolesOfAnAccount> GetRolesOfAnAccountByEmployeeId(Guid id);
}