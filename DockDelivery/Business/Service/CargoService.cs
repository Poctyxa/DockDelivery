using DockDelivery.Business.Abstract;
using DockDelivery.Domain.Entities;
using DockDelivery.Domain.UoW;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DockDelivery.Business.Service
{
    public class CargoService : ICargoService
    {
        private readonly IDockDeliveryUnitOfWork uow;

        public CargoService(IDockDeliveryUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<Cargo> CreateAsync(Cargo cargo)
        {
            try
            {
                cargo.Id = Guid.NewGuid();
                var newCargo = await uow.Cargos.CreateAsync(cargo);
                await uow.SaveAllAsync();

                return newCargo;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<Cargo> GetByIdAsync(Guid cargoId)
        {
            return await uow.Cargos.GetByIdAsync(cargoId);
        }

        public async Task<IEnumerable<Cargo>> GetCargosAsync()
        {
            return await uow.Cargos.GetAllAsync();
        }

        public async Task<Cargo> RemoveAsync(Guid cargoId)
        {
            try
            {
                var removedCargo = await uow.Cargos.RemoveAsync(cargoId);
                await uow.SaveAllAsync();

                return removedCargo;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<Cargo> UpdateAsync(Cargo cargo)
        {
            try
            {
                var updatedCargo = await uow.Cargos.UpdateAsync(cargo);
                await uow.SaveAllAsync();

                return updatedCargo;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
