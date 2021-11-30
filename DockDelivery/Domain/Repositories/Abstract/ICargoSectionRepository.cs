using DockDelivery.Domain.Entities;
using System.Threading.Tasks;

namespace DockDelivery.Domain.Repositories.Abstract
{
    public interface ICargoSectionRepository : IEntityBaseRepository<CargoSection>
    {
        Task<CargoSection> LoadCargosAsync(CargoSection cargoSection);
        Task<CargoSection> LoadDepartmentAsync(CargoSection cargoSection);
        Task<CargoSection> LoadCargoTypeAsync(CargoSection cargoSection);
    }
}
