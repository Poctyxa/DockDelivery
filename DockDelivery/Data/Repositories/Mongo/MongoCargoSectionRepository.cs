using DockDelivery.Domain.Context;
using DockDelivery.Domain.Entities;
using DockDelivery.Domain.Repositories.Abstract;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DockDelivery.Domain.Repositories.Mongo
{
    public class MongoCargoSectionRepository : ICargoSectionRepository
    {
        private readonly IMongoDockDeliveryDbContext context;

        public MongoCargoSectionRepository(IMongoDockDeliveryDbContext context)
        {
            this.context = context;
        }

        public async Task<CargoSection> CreateAsync(CargoSection entity)
        {
            await context.CargoSections.InsertOneAsync(entity);
            return entity;
        }

        public async Task<IEnumerable<CargoSection>> FindAsync(Expression<Func<CargoSection, bool>> expression)
        {
            return await context.CargoSections.Find(expression).ToListAsync();
        }

        public async Task<IEnumerable<CargoSection>> GetAllAsync()
        {
            return await context.CargoSections.AsQueryable().ToListAsync();
        }

        public async Task<CargoSection> GetByIdAsync(string entityId)
        {
            var filter = Builders<CargoSection>.Filter.Eq("Id", entityId);
            return await context.CargoSections.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<CargoSection> LoadCargosAsync(CargoSection cargoSection)
        {
            var filter = Builders<Cargo>.Filter.Eq("cargoSectionId", cargoSection.Id);
            var cargos = await context.Cargos.Find(filter).ToListAsync();
            cargoSection.Cargos = cargos;
            return cargoSection;
        }

        public async Task<CargoSection> LoadCargoTypeAsync(CargoSection cargoSection)
        {
            var filter = Builders<CargoType>.Filter.Eq("Id", cargoSection.CargoTypeId);
            var cargoType = (await context.CargoTypes.FindAsync(filter)).First();
            cargoSection.CargoType = cargoType;
            return cargoSection;
        }

        public async Task<CargoSection> LoadDepartmentAsync(CargoSection cargoSection)
        {
            var filter = Builders<Department>.Filter.Eq("Id", cargoSection.DepartmentId);
            var department = (await context.Departments.FindAsync(filter)).First();
            cargoSection.Department = department;
            return cargoSection;
        }

        public async Task<CargoSection> RemoveAsync(string entityId)
        {
            var filter = Builders<CargoSection>.Filter.Eq("Id", entityId);
            return await context.CargoSections.FindOneAndDeleteAsync(filter);
        }

        public async Task<bool> RemoveRangeAsync(IEnumerable<CargoSection> entities)
        {
            var ids = entities.Select(e => e.Id).ToList();
            var filter = Builders<CargoSection>.Filter.In("Id", ids);
            var res = await context.CargoSections.DeleteManyAsync(filter);
            return res.DeletedCount > 0 ? true : false;
        }

        public async Task<CargoSection> UpdateAsync(CargoSection entity)
        {
            var filter = Builders<CargoSection>.Filter.Eq("Id", entity.Id);
            var update = Builders<CargoSection>.Update
                .Set(p => p.DepartmentId, entity.DepartmentId)
                .Set(p => p.CargoTypeId , entity.CargoTypeId)
                .Set(p => p.WeightLimit, entity.WeightLimit)
                .Set(p => p.CapacityLimit, entity.CapacityLimit);

            var updateRes = await context.CargoSections.UpdateOneAsync(filter, update);
            return entity;
        }
    }
}
