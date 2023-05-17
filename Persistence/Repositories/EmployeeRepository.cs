using Application.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories;

public class EmployeeRepository : BaseRepository<Employee, ApplicationDbContext>, IEmployeeRepository
{
    public EmployeeRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Employee?> FindOneByNikIncludeAccount(string Nik)
    {
        return await _set.Include(e => e.Account).FirstOrDefaultAsync(e => e.Nik.Equals(Nik));
    }
}