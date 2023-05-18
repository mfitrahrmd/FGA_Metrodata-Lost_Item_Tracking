#region

using Domain.Entities;

#endregion

namespace Application.Repositories;

public interface IEmployeeRepository : IBaseRepository<Employee>
{
    Task<Employee?> FindOneByNikIncludeAccount(string Nik);
    Task<bool> IsEmployeeUnique(Employee employee);
}