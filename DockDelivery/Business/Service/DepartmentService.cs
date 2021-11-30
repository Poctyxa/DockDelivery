using DockDelivery.Business.Abstract;
using DockDelivery.Domain.Entities;
using DockDelivery.Domain.UoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DockDelivery.Business.Service
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDockDeliveryUnitOfWork uow;

        public DepartmentService(IDockDeliveryUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<Department> CreateAsync(Department department)
        {
            try 
            {
                department.Id = Guid.NewGuid();
                var newDepartment = await uow.Departments.CreateAsync(department);
                await uow.SaveAllAsync();

                return newDepartment;
            } 
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }       
        }

        public async Task<Department> GetByIdAsync(Guid departmentId)
        {
            return await uow.Departments.GetByIdAsync(departmentId);
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

                await uow.SaveAllAsync();

                return true;
            }
            catch 
            {
                return false;
            } 
        }

        public async Task<IEnumerable<Department>> GetDepartmentsAsync()
        {
            return await uow.Departments.GetAllAsync();
        }

        public async Task<Department> RemoveAsync(Guid departmentId)
        {
            try
            {
                var removedDepartment = await uow.Departments.RemoveAsync(departmentId);
                await uow.SaveAllAsync();

                return removedDepartment;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<Department> UpdateAsync(Department department)
        {
            try
            {
                var updatedDepartment = await uow.Departments.UpdateAsync(department);
                await uow.SaveAllAsync();

                return updatedDepartment;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<IEnumerable<Department>> GetCapableDepartmentsAsync(
            Guid cargoTypeId, 
            double weight, 
            double capacity, 
            DateTime maxDate)
        {
            List<CargoSection> sections = (await uow.CargoSections.FindAsync(cs => cs.CargoTypeId == cargoTypeId)).ToList();
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

            await uow.SaveAllAsync();

            return capableDepartments;
        }
    }
}
