using DockDelivery.Business.Abstract;
using DockDelivery.Domain.Entities;
using DockDelivery.Domain.UoW;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DockDelivery.Business.Service
{
    public class CargoTypeService : ICargoTypeService
    {
        private readonly IDockDeliveryUnitOfWork uow;

        public CargoTypeService(IDockDeliveryUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<CargoType> CreateAsync(CargoType cargoType)
        {
            try
            {
                cargoType.Id = ObjectId.GenerateNewId().ToString(); ;
                var newCargoType = await uow.CargoTypes.CreateAsync(cargoType);
                await uow.SaveAllAsync();

                return newCargoType;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<CargoType> GetByIdAsync(string cargoTypeId)
        {
            return await uow.CargoTypes.GetByIdAsync(cargoTypeId);
        }

        public async Task<IEnumerable<CargoType>> GetCargoTypesAsync()
        {
            return await uow.CargoTypes.GetAllAsync();
        }

        public async Task<CargoType> RemoveAsync(string cargoTypeId)
        {
            try
            {
                var removedCargoType = await uow.CargoTypes.RemoveAsync(cargoTypeId);
                await uow.SaveAllAsync();

                return removedCargoType;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<CargoType> UpdateAsync(CargoType cargoType)
        {
            try
            {
                var updatedCargoType = await uow.CargoTypes.UpdateAsync(cargoType);
                await uow.SaveAllAsync();

                return updatedCargoType;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
