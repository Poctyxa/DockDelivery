using DockDelivery.Domain.Context;
using DockDelivery.Domain.Repositories.Abstract;
using DockDelivery.Domain.Repositories.EntityFramework;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DockDelivery.Domain.UoW
{
    public class DockDeliveryUnitOfWork : IDockDeliveryUnitOfWork
    {
        private readonly IDockDeliveryDbContext context;
        private IDepartmentRepository departmentRepository;
        private ICargoRepository cargoRepository;
        private ICargoSectionRepository cargoSectionRepository;
        private ICargoTypeRepository cargoTypeRepository;

        public DockDeliveryUnitOfWork(IDockDeliveryDbContext context)
        {
            this.context = context;
        }

        public IDepartmentRepository Departments => departmentRepository ??= new EFDepartmentRepository(context);

        public ICargoRepository Cargos => cargoRepository ??= new EFCargoRepository(context);

        public ICargoSectionRepository CargoSections => cargoSectionRepository ??= new EFCargoSectionRepository(context);

        public ICargoTypeRepository CargoTypes => cargoTypeRepository ??= new EFCargoTypeRepository(context);

        public Task SaveAllAsync()
        {
            return context.SaveAll();
        }
    }
}
