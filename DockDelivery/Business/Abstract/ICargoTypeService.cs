using DockDelivery.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DockDelivery.Business.Abstract
{
    public interface ICargoTypeService
    {
        Task<IEnumerable<CargoType>> GetCargoTypesAsync();
        Task<CargoType> GetByIdAsync(string cargoTypeId);
        Task<CargoType> CreateAsync(CargoType cargoType);
        Task<CargoType> UpdateAsync(CargoType cargoType);
        Task<CargoType> RemoveAsync(string cargoTypeId);
    }
}
