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

    public async Task<bool> IsEmployeeUnique(Employee employee)
    {
        IQueryable<Employee> result;

        if (employee.PhoneNumber is not null)
        {
            result = _set.Where(e =>
                employee.Nik.Equals(e.Nik) || employee.Email.Equals(e.Email) || employee.PhoneNumber.Equals(e.PhoneNumber));
            
            return !result.Any();
        }

        result = _set.Where(e =>
            e.Nik.Equals(employee.Nik) || e.Email.Equals(employee.Email));
        
        return !result.Any();
    }
}