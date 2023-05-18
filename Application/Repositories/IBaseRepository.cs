#region

using Domain.Common;

#endregion

namespace Application.Repositories;

public interface IBaseRepository<TEntity>
    where TEntity : BaseEntity
{
    Task InsertOneAsync(TEntity entity);
    Task UpdateOneAsync(TEntity entity);
    Task DeleteOneAsync(TEntity entity);
    Task<TEntity?> FindOneByIdAsync(Guid id);
    Task<ICollection<TEntity>> FindAllAsync();
    Task<bool> IsExistAsync(Guid id);
}