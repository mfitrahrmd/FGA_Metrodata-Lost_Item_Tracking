#region

using Application.Repositories;
using Domain.Common;
using Microsoft.EntityFrameworkCore;

#endregion

namespace Persistence.Repositories;

public class BaseRepository<TEntity, TContext> : IBaseRepository<TEntity>
    where TEntity : BaseEntity
    where TContext : DbContext
{
    protected readonly TContext _context;
    protected readonly DbSet<TEntity> _set;

    public BaseRepository(TContext context)
    {
        _context = context;
        _set = context.Set<TEntity>();
    }

    public async Task<TEntity> InsertOneAsync(TEntity entity)
    {
        await _set.AddAsync(entity);

        await _context.SaveChangesAsync();

        return entity;
    }

    public async Task<TEntity> UpdateOneByIdAsync(Guid id, TEntity entity)
    {
        var foundEntity = await FindOneByIdAsNoTracking(id);

        if (foundEntity is null)
            throw new Exception($"{typeof(TEntity).Name} with id '{id}' was not found.");

        entity.Id = foundEntity.Id;
        
        _context.Set<TEntity>().Update(entity);

        await _context.SaveChangesAsync();

        return entity;
    }

    public async Task<TEntity> DeleteOneByIdAsync(Guid id)
    {
        var foundEntity = await _set.FindAsync(id);

        _set.Remove(foundEntity);

        await _context.SaveChangesAsync();

        return foundEntity;
    }

    public async Task<TEntity?> FindOneByIdAsync(Guid id)
    {
        return await _set.FirstOrDefaultAsync(e => e.Id.Equals(id));
    }

    public async Task<ICollection<TEntity>> FindAllAsync()
    {
        return await _set.ToListAsync();
    }

    public async Task<bool> IsExistAsync(Guid id)
    {
        return _set.AsNoTracking().Any(e => e.Id.Equals(id));
    }

    public async Task<TEntity?> FindOneByIdAsNoTracking(Guid id)
    {
        var foundEntity = await _set.FindAsync(id);

        if (foundEntity is null)
        {
            return null;
        }

        _context.Entry(foundEntity).State = EntityState.Detached;

        return foundEntity;
    }
}