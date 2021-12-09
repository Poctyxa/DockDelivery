using DockDelivery.Business.Abstract;
using DockDelivery.Domain.Entities;
using DockDelivery.Domain.UoW.Mongo;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DockDelivery.Business.Service.Mongo
{
    public class MongoCargoSectionService : ICargoSectionService
    {
        private readonly IMongoDockDeliveryUnitOfWork uow;

        public MongoCargoSectionService(IMongoDockDeliveryUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<CargoSection> CreateAsync(CargoSection cargoSection)
        {
            try
            {
                cargoSection.Id = ObjectId.GenerateNewId().ToString();
                await uow.CargoSections.CreateAsync(cargoSection);

                return cargoSection;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<CargoSection> GetByIdAsync(string cargoSectionId)
        {
            return await uow.CargoSections.GetByIdAsync(cargoSectionId);
        }

        public async Task<IEnumerable<CargoSection>> GetCargoSectionsAsync()
        {
            return await uow.CargoSections.GetAllAsync();
        }

        public async Task<CargoSection> RemoveAsync(string cargoSectionId)
        {
            return await uow.CargoSections.RemoveAsync(cargoSectionId);
        }

        public async Task<CargoSection> UpdateAsync(CargoSection cargoSection)
        {
            return await uow.CargoSections.UpdateAsync(cargoSection);
        }
    }
}
