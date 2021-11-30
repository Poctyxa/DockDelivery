using DockDelivery.Domain.Context;
using DockDelivery.Domain.Entities;
using DockDelivery.Domain.Repositories.Abstract;

namespace DockDelivery.Domain.Repositories.EntityFramework
{
    public class EFCargoTypeRepository : EFEntityBaseRepository<CargoType>, ICargoTypeRepository
    {
        public EFCargoTypeRepository(IDockDeliveryDbContext context) : base(context)
        {
        }
    }
}
