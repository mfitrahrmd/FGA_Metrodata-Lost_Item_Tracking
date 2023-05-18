#region

using Application.Repositories;
using Domain.Entities;
using Persistence.Context;

#endregion

namespace Persistence.Repositories;

public class DepartmentRepository : BaseRepository<Department, ApplicationDbContext>, IDepartmentRepository
{
    public DepartmentRepository(ApplicationDbContext context) : base(context)
    {
    }
}