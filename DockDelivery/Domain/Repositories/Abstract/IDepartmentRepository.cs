using DockDelivery.Domain.Entities;
using System.Threading.Tasks;

namespace DockDelivery.Domain.Repositories.Abstract
{
    public interface IDepartmentRepository : IEntityBaseRepository<Department>
    {
        Task<Department> LoadSectionsAsync(Department department);
    }
}
