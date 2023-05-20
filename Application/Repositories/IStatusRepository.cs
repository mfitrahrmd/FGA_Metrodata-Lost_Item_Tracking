using Domain.Entities;

namespace Application.Repositories;

public interface IStatusRepository : IBaseRepository<Status>
{
    Task<Status> FindOrCreateStatusAsync(Status status);
}