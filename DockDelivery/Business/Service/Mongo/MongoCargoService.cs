using DockDelivery.Business.Abstract;
using DockDelivery.Domain.Entities;
using DockDelivery.Domain.UoW.Mongo;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DockDelivery.Business.Service.Mongo
{
    public class MongoCargoService : ICargoService
    {
        private readonly IMongoDockDeliveryUnitOfWork uow;

        public MongoCargoService(IMongoDockDeliveryUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<Cargo> CreateAsync(Cargo cargo)
        {
            try
            {
                cargo.Id = ObjectId.GenerateNewId().ToString();
                await uow.Cargos.CreateAsync(cargo);

                return cargo;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }   
        }

        public async Task<Cargo> GetByIdAsync(string cargoId)
        {
            return await uow.Cargos.GetByIdAsync(cargoId);
        }

        public async Task<IEnumerable<Cargo>> GetCargosAsync()
        {
            return await uow.Cargos.GetAllAsync();
        }

        public async Task<Cargo> RemoveAsync(string cargoId)
        {
            return await uow.Cargos.RemoveAsync(cargoId);
        }

        public async Task<Cargo> UpdateAsync(Cargo cargo)
        {
            return await uow.Cargos.UpdateAsync(cargo);
        }
    }
}
