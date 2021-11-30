using DockDelivery.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DockDelivery.Business.Abstract
{
    public interface ICargoTypeService
    {
        Task<IEnumerable<CargoType>> GetCargoTypesAsync();
        Task<CargoType> GetByIdAsync(Guid cargoTypeId);
        Task<CargoType> CreateAsync(CargoType cargoType);
        Task<CargoType> UpdateAsync(CargoType cargoType);
        Task<CargoType> RemoveAsync(Guid cargoTypeId);
    }
}
