using DockDelivery.Domain.Repositories.Abstract;
using System.Threading.Tasks;

namespace DockDelivery.Domain.UoW
{
    public interface IDockDeliveryUnitOfWork
    {
        IDepartmentRepository Departments { get; }
        ICargoRepository Cargos { get; }
        ICargoSectionRepository CargoSections { get; }
        ICargoTypeRepository CargoTypes { get; }
        Task SaveAllAsync();
    }
}
