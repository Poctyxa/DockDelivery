using DockDelivery.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DockDelivery.Business.Abstract
{
    public interface ICargoService
    {
        Task<IEnumerable<Cargo>> GetCargosAsync();
        Task<Cargo> GetByIdAsync(string cargoId);
        Task<Cargo> CreateAsync(Cargo cargo);
        Task<Cargo> UpdateAsync(Cargo cargo);
        Task<Cargo> RemoveAsync(string cargoId);
    }
}
