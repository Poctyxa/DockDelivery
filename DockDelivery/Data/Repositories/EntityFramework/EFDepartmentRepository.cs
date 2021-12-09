using DockDelivery.Domain.Context;
using DockDelivery.Domain.Entities;
using DockDelivery.Domain.Repositories.Abstract;
using System.Threading.Tasks;

namespace DockDelivery.Domain.Repositories.EntityFramework
{
    public class EFDepartmentRepository : EFEntityBaseRepository<Department>, IDepartmentRepository
    {
        private readonly IDockDeliveryDbContext context;

        public EFDepartmentRepository(IDockDeliveryDbContext context) : base(context)
        {
            this.context = context;
        }

        public Task<Department> LoadSectionsAsync(Department department)
        {
            context.Entry(department).Collection("CargoSections").Load();
            return Task.FromResult(department);
        }
    }
}
