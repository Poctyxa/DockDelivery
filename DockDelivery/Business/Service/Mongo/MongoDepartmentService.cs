using DockDelivery.Business.Abstract;
using DockDelivery.Domain.Entities;
using DockDelivery.Domain.UoW.Mongo;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DockDelivery.Business.Service.Mongo
{
    public class MongoDepartmentService : IDepartmentService
    {
        private readonly IMongoDockDeliveryUnitOfWork uow;

        public MongoDepartmentService(IMongoDockDeliveryUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<bool> ClearSectionsAsync(Department department)
        {
            try
            {
                await uow.Departments.LoadSectionsAsync(department);

                var sections = department
                    .CargoSections
                    .ToList();

                foreach (var section in sections)
                {
                    await uow.CargoSections.LoadCargosAsync(section);
                    section.Cargos = new List<Cargo>();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Department> CreateAsync(Department department)
        {
            try
            {
                department.Id = ObjectId.GenerateNewId().ToString();
                await uow.Departments.CreateAsync(department);
 
                return department;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<Department> GetByIdAsync(string departmentId)
        {
            return await uow.Departments.GetByIdAsync(departmentId);
        }

        public async Task<IEnumerable<Department>> GetCapableDepartmentsAsync(string cargoTypeId, double weight, double capacity, DateTime maxDate)
        {
            var sectionFilter = Builders<CargoSection>.Filter.Eq("cargoTypeId", cargoTypeId);

            List<CargoSection> sections =
                (await uow.CargoSections.FindAsync(cs => cs.CargoTypeId == cargoTypeId)).ToList();

            List<Department> capableDepartments = new List<Department>();

            foreach (var section in sections)
            {
                await uow.CargoSections.LoadCargosAsync(section);
                await uow.CargoSections.LoadDepartmentAsync(section);
                await uow.CargoSections.LoadCargoTypeAsync(section);

                double sumCapacity = section.Cargos.Select(c => c.Capacity).Sum();
                double leftCapacity = section.CapacityLimit - sumCapacity;

                double sumWeight = section.Cargos.Select(c => c.Weight).Sum();
                double leftWeight = section.WeightLimit - sumWeight;

                if (leftCapacity >= capacity
                    && leftWeight >= weight
                    && section.Department.NextSending != null
                    && section.Department.NextSending >= maxDate)
                    capableDepartments.Add(section.Department);
            }

            return capableDepartments;
        }

        public async Task<IEnumerable<Department>> GetDepartmentsAsync()
        {
            var departments = await uow.Departments.GetAllAsync();

            return departments;
        }

        public async Task<Department> RemoveAsync(string departmentId)
        {
            return await uow.Departments.RemoveAsync(departmentId);
        }

        public async Task<Department> UpdateAsync(Department department)
        {
            return await uow.Departments.UpdateAsync(department);
        }
    }
}
