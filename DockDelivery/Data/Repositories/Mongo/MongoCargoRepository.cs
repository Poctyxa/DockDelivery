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
    public class MongoCargoRepository : ICargoRepository
    {
        private readonly IMongoDockDeliveryDbContext context;

        public MongoCargoRepository(IMongoDockDeliveryDbContext context)
        {
            this.context = context;
        }

        public async Task<Cargo> CreateAsync(Cargo entity)
        {
            await context.Cargos.InsertOneAsync(entity);
            return entity;
        }

        public async Task<IEnumerable<Cargo>> FindAsync(Expression<Func<Cargo, bool>> expression)
        {
            return await context.Cargos.Find(expression).ToListAsync();
        }

        public async Task<IEnumerable<Cargo>> GetAllAsync()
        {
            return await context.Cargos.AsQueryable().ToListAsync();
        }

        public async Task<Cargo> GetByIdAsync(string entityId)
        {
            var filter = Builders<Cargo>.Filter.Eq("Id", entityId);
            return await context.Cargos.Find(filter).FirstOrDefaultAsync();
        }

        public Task<CargoSection> LoadCargoSectionAsync(CargoSection cargoSection)
        {
            throw new NotImplementedException();
        }

        public async Task<Cargo> RemoveAsync(string entityId)
        {
            var filter = Builders<Cargo>.Filter.Eq("Id", entityId);
            return await context.Cargos.FindOneAndDeleteAsync(filter);
        }

        public async Task<bool> RemoveRangeAsync(IEnumerable<Cargo> entities)
        {
            var ids = entities.Select(e => e.Id).ToList();
            var filter = Builders<Cargo>.Filter.In("Id", ids);
            var res = await context.Cargos.DeleteManyAsync(filter);
            return res.DeletedCount > 0 ? true : false;
        }

        public async Task<Cargo> UpdateAsync(Cargo entity)
        {
            var filter = Builders<Cargo>.Filter.Eq("Id", entity.Id);
            var update = Builders<Cargo>.Update
                .Set(p => p.CargoSectionId, entity.CargoSectionId)
                .Set(p => p.Description, entity.Description)
                .Set(p => p.Weight, entity.Weight)
                .Set(p => p.Owner, entity.Owner)
                .Set(p => p.Capacity, entity.Capacity);

            var updateRes = await context.Cargos.UpdateOneAsync(filter, update);
            return entity;
        }
    }
}
