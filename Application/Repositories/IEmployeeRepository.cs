using Domain.Entities;

namespace Application.Repositories;

public interface IEmployeeRepository : IBaseRepository<Employee>
{
    Task<Employee?> FindOneByNikIncludeAccount(string Nik);
}