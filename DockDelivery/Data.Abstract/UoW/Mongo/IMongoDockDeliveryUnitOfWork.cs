using DockDelivery.Domain.Repositories.Abstract;

namespace DockDelivery.Domain.UoW.Mongo
{
    public interface IMongoDockDeliveryUnitOfWork
    {
        IDepartmentRepository Departments { get; }
        ICargoRepository Cargos { get; }
        ICargoSectionRepository CargoSections { get; }
        ICargoTypeRepository CargoTypes { get; }
    }
}
