using DockDelivery.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DockDelivery.Business.Abstract
{
    public interface IDepartmentService
    {
        Task<IEnumerable<Department>> GetDepartmentsAsync();
        Task<Department> GetByIdAsync(string departmentId);
        Task<Department> CreateAsync(Department department);
        Task<Department> UpdateAsync(Department department);
        Task<Department> RemoveAsync(string departmentId);
        Task<bool> ClearSectionsAsync(Department department);
        Task<IEnumerable<Department>> GetCapableDepartmentsAsync(string cargoTypeId, double weight, double capacity, DateTime maxDate);
    }
}
