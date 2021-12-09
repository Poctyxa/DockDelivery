using DockDelivery.Business.Abstract;
using DockDelivery.Domain.Entities;
using DockDelivery.Domain.UoW.Mongo;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DockDelivery.Business.Service.Mongo
{
    public class MongoCargoTypeService : ICargoTypeService
    {
        private readonly IMongoDockDeliveryUnitOfWork uow;

        public MongoCargoTypeService(IMongoDockDeliveryUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<CargoType> CreateAsync(CargoType cargoType)
        {
            try
            {
                cargoType.Id = ObjectId.GenerateNewId().ToString();
                await uow.CargoTypes.CreateAsync(cargoType);

                return cargoType;
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
            return await uow.CargoTypes.RemoveAsync(cargoTypeId);
        }

        public async Task<CargoType> UpdateAsync(CargoType cargoType)
        {
            return await uow.CargoTypes.UpdateAsync(cargoType);
        }
    }
}
