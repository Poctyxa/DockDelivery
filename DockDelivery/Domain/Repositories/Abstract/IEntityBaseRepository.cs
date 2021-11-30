using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DockDelivery.Domain.Repositories.Abstract
{
    public interface IEntityBaseRepository<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> expression);
        Task<TEntity> GetByIdAsync(Guid entityId);
        Task<TEntity> CreateAsync(TEntity entity);
        Task<TEntity> RemoveAsync(Guid entityId);
        Task<bool> RemoveRangeAsync(IEnumerable<TEntity> entities);
        Task<TEntity> UpdateAsync(TEntity entity);
    }
}
