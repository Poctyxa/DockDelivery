using DockDelivery.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DockDelivery.Business.Abstract
{
    public interface ICargoSectionService
    {
        Task<IEnumerable<CargoSection>> GetCargoSectionsAsync();
        Task<CargoSection> GetByIdAsync(Guid cargoSectionId);
        Task<CargoSection> CreateAsync(CargoSection cargoSection);
        Task<CargoSection> UpdateAsync(CargoSection cargoSection);
        Task<CargoSection> RemoveAsync(Guid cargoSectionId);
    }
}
