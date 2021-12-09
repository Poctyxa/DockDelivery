using DockDelivery.Domain.Context;
using DockDelivery.Domain.Entities;
using DockDelivery.Domain.Repositories.Abstract;
using System.Threading.Tasks;

namespace DockDelivery.Domain.Repositories.EntityFramework
{
    public class EFCargoSectionRepository : EFEntityBaseRepository<CargoSection>, ICargoSectionRepository
    {
        private readonly IDockDeliveryDbContext context;

        public EFCargoSectionRepository(IDockDeliveryDbContext context) : base(context)
        {
            this.context = context;
        }

        public Task<CargoSection> LoadCargosAsync(CargoSection cargoSection)
        {
            context.Entry(cargoSection).Collection("Cargos").Load();
            return Task.FromResult(cargoSection);
        }

        public Task<CargoSection> LoadDepartmentAsync(CargoSection cargoSection)
        {
            context.Entry(cargoSection).Reference("Department").Load();
            return Task.FromResult(cargoSection);
        }

        public Task<CargoSection> LoadCargoTypeAsync(CargoSection cargoSection)
        {
            context.Entry(cargoSection).Reference("CargoType").Load();
            return Task.FromResult(cargoSection);
        }
    }
}
