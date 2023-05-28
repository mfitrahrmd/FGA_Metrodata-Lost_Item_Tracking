#region

using Domain.Common;

#endregion

namespace Application.Repositories;

public interface IBaseRepository<TEntity>
    where TEntity : BaseEntity
{
    Task<TEntity> InsertOneAsync(TEntity entity);
    Task<TEntity> UpdateOneByIdAsync(Guid id, TEntity entity);
    Task<TEntity> DeleteOneByIdAsync(Guid id);
    Task<TEntity?> FindOneByIdAsync(Guid id);
    Task<ICollection<TEntity>> FindAllAsync();
    Task<bool> IsExistAsync(Guid id);
}