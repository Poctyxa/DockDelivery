using DockDelivery.Business.Abstract;
using DockDelivery.Domain.Entities;
using DockDelivery.Domain.UoW;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DockDelivery.Business.Service
{
    public class CargoSectionService : ICargoSectionService
    {
        private readonly IDockDeliveryUnitOfWork uow;

        public CargoSectionService(IDockDeliveryUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<CargoSection> CreateAsync(CargoSection cargoSection)
        {
            try
            {
                cargoSection.Id = ObjectId.GenerateNewId().ToString(); ;
                var newCargoSection = await uow.CargoSections.CreateAsync(cargoSection);
                await uow.SaveAllAsync();

                return newCargoSection;
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
            try
            {
                var removedCargoSection = await uow.CargoSections.RemoveAsync(cargoSectionId);
                await uow.SaveAllAsync();

                return removedCargoSection;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<CargoSection> UpdateAsync(CargoSection cargoSection)
        {
            try
            {
                var updatedCargoSection = await uow.CargoSections.UpdateAsync(cargoSection);
                await uow.SaveAllAsync();

                return updatedCargoSection;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
