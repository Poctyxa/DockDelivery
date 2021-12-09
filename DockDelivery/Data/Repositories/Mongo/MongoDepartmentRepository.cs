using System.Linq;
using DockDelivery.Domain.Context;
using DockDelivery.Domain.Entities;
using DockDelivery.Domain.Repositories.Abstract;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace DockDelivery.Domain.Repositories.Mongo
{
    public class MongoDepartmentRepository : IDepartmentRepository
    {
        private readonly IMongoDockDeliveryDbContext context;

        public MongoDepartmentRepository(IMongoDockDeliveryDbContext context)
        {
            this.context = context;
        }

        public async Task<Department> CreateAsync(Department entity)
        {
            // Make sections empty
            entity.CargoSections = new List<CargoSection>();
            await context.Departments.InsertOneAsync(entity);
            return entity;
        }

        public async Task<IEnumerable<Department>> FindAsync(Expression<Func<Department, bool>> expression)
        {
            return await context.Departments.Find(expression).ToListAsync();
        }

        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            return await context.Departments.AsQueryable().ToListAsync();
        }

        public async Task<Department> GetByIdAsync(string entityId)
        {
            var filter = Builders<Department>.Filter.Eq("Id", entityId);
            return await context.Departments.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<Department> LoadSectionsAsync(Department department)
        {
            var filter = Builders<CargoSection>.Filter.Eq("departmentId", department.Id);
            var sections = await context.CargoSections.Find(filter).ToListAsync();
            department.CargoSections = sections;
            return department;
        }

        public async Task<Department> RemoveAsync(string entityId)
        {
            var filter = Builders<Department>.Filter.Eq("Id", entityId);
            return await context.Departments.FindOneAndDeleteAsync(filter);
        }

        public async Task<bool> RemoveRangeAsync(IEnumerable<Department> entities)
        {
            var ids = entities.Select(e => e.Id).ToList();
            var filter = Builders<Department>.Filter.In("Id", ids);
            var res = await context.Departments.DeleteManyAsync(filter);
            return res.DeletedCount > 0 ? true: false;
        }

        public async Task<Department> UpdateAsync(Department entity)
        {
            var filter = Builders<Department>.Filter.Eq("Id", entity.Id);
            var update = Builders<Department>.Update
                .Set(p => p.DepartmentName, entity.DepartmentName)
                .Set(p => p.DepartmentAddress, entity.DepartmentAddress)
                .Set(p => p.LastSending, entity.LastSending)
                .Set(p => p.NextSending, entity.NextSending)
                .Set(p => p.CargoSections, entity.CargoSections);

            var updateRes = await context.Departments.UpdateOneAsync(filter, update);
            return entity;
        }
    }
}
