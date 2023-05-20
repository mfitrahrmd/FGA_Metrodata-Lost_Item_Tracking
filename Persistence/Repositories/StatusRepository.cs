using Application.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories;

public class StatusRepository : BaseRepository<Status, ApplicationDbContext>, IStatusRepository
{
    public StatusRepository(ApplicationDbContext context) : base(context)
    {
    }
    
    public async Task<Status> FindOrCreateStatusAsync(Status status)
    {
        var foundStatus = await _set.FirstOrDefaultAsync(s => s.Name.Equals(status.Name));

        if (foundStatus is null)
        {
            var newStatus = status;
            
            var res = await _set.AddAsync(newStatus);

            return res.Entity;
        }

        return foundStatus;
    }
}