using DockDelivery.Domain.Context;
using DockDelivery.Domain.Repositories.Abstract;
using DockDelivery.Domain.Repositories.Mongo;

namespace DockDelivery.Domain.UoW.Mongo
{
    public class MongoDockDeliveryUnitOfWork : IMongoDockDeliveryUnitOfWork
    {
        private readonly IMongoDockDeliveryDbContext context;
        private IDepartmentRepository departmentRepository;
        private ICargoRepository cargoRepository;
        private ICargoSectionRepository cargoSectionRepository;
        private ICargoTypeRepository cargoTypeRepository;

        public MongoDockDeliveryUnitOfWork(IMongoDockDeliveryDbContext context)
        {
            this.context = context;
        }

        public IDepartmentRepository Departments => departmentRepository ??= new MongoDepartmentRepository(context);

        public ICargoRepository Cargos => cargoRepository ??= new MongoCargoRepository(context);

        public ICargoSectionRepository CargoSections => cargoSectionRepository ??= new MongoCargoSectionRepository(context);

        public ICargoTypeRepository CargoTypes => cargoTypeRepository  ??= new MongoCargoTypeRepository(context);
    }
}
