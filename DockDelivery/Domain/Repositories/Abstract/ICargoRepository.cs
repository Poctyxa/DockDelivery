using DockDelivery.Domain.Entities;
using System.Threading.Tasks;

namespace DockDelivery.Domain.Repositories.Abstract
{
    public interface ICargoRepository : IEntityBaseRepository<Cargo>
    {
        public Task<CargoSection> LoadCargoSectionAsync(CargoSection cargoSection);
    }
}
