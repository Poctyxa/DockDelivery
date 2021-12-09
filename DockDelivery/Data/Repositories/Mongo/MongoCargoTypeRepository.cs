using DockDelivery.Domain.Context;
using DockDelivery.Domain.Entities;
using DockDelivery.Domain.Repositories.Abstract;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DockDelivery.Domain.Repositories.Mongo
{
    public class MongoCargoTypeRepository : ICargoTypeRepository
    {
        private readonly IMongoDockDeliveryDbContext context;

        public MongoCargoTypeRepository(IMongoDockDeliveryDbContext context)
        {
            this.context = context;
        }

        public async Task<CargoType> CreateAsync(CargoType entity)
        {
            await context.CargoTypes.InsertOneAsync(entity);
            return entity;
        }

        public async Task<IEnumerable<CargoType>> FindAsync(Expression<Func<CargoType, bool>> expression)
        {
            return await context.CargoTypes.Find(expression).ToListAsync();
        }

        public async Task<IEnumerable<CargoType>> GetAllAsync()
        {
            return await context.CargoTypes.AsQueryable().ToListAsync();
        }

        public async Task<CargoType> GetByIdAsync(string entityId)
        {
            var filter = Builders<CargoType>.Filter.Eq("Id", entityId);
            return await context.CargoTypes.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<CargoType> RemoveAsync(string entityId)
        {
            var filter = Builders<CargoType>.Filter.Eq("Id", entityId);
            return await context.CargoTypes.FindOneAndDeleteAsync(filter);
        }

        public async Task<bool> RemoveRangeAsync(IEnumerable<CargoType> entities)
        {
            var ids = entities.Select(e => e.Id).ToList();
            var filter = Builders<CargoType>.Filter.In("Id", ids);
            var res = await context.CargoTypes.DeleteManyAsync(filter);
            return res.DeletedCount > 0 ? true : false;
        }

        public async Task<CargoType> UpdateAsync(CargoType entity)
        {
            var filter = Builders<CargoType>.Filter.Eq("Id", entity.Id);
            var update = Builders<CargoType>.Update
                .Set(p => p.TypeName, entity.TypeName);

            var updateRes = await context.CargoTypes.UpdateOneAsync(filter, update);
            return entity;
        }
    }
}
