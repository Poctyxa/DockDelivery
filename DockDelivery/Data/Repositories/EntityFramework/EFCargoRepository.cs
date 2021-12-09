using DockDelivery.Domain.Context;
using DockDelivery.Domain.Entities;
using DockDelivery.Domain.Repositories.Abstract;
using System.Threading.Tasks;

namespace DockDelivery.Domain.Repositories.EntityFramework
{
    public class EFCargoRepository : EFEntityBaseRepository<Cargo>, ICargoRepository
    {
        private readonly IDockDeliveryDbContext context;

        public EFCargoRepository(IDockDeliveryDbContext context) : base(context)
        {
            this.context = context;
        }

        public Task<CargoSection> LoadCargoSectionAsync(CargoSection cargoSection)
        {
            context.Entry(cargoSection).Reference("Department").Load();
            return Task.FromResult(cargoSection);
        }
    }
}
