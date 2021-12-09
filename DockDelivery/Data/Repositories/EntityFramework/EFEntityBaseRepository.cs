using DockDelivery.Domain.Context;
using DockDelivery.Domain.Entities;
using DockDelivery.Domain.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DockDelivery.Domain.Repositories.EntityFramework
{
    public abstract class EFEntityBaseRepository<TEntity> : IEntityBaseRepository<TEntity> where TEntity : EntityBase
    {
        private readonly IDockDeliveryDbContext context;
        public EFEntityBaseRepository(IDockDeliveryDbContext context)
        {
            this.context = context;
        }

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            await context.Set<TEntity>().AddAsync(entity);
            return entity;
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await context.Set<TEntity>().Where(expression).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await context.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(string entityId)
        {
            return await context.Set<TEntity>().FirstAsync(en => en.Id == entityId);
        }

        public async Task<TEntity> RemoveAsync(string entityId)
        {
            var entityObj = await context.Set<TEntity>().FirstAsync(en => en.Id == entityId);
            context.Set<TEntity>().Remove(entityObj);
            return entityObj;
        }

        public Task<bool> RemoveRangeAsync(IEnumerable<TEntity> entities)
        {
            try
            {
                context.Set<TEntity>().RemoveRange(entities);
                return Task.FromResult(true);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }

        public Task<TEntity> UpdateAsync(TEntity entity)
        {
            context.Set<TEntity>().Update(entity);
            return Task.FromResult(entity);
        }
    }
}
