using DockDelivery.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading.Tasks;

namespace DockDelivery.Domain.Context
{
    public interface IDockDeliveryDbContext
    {
        DbSet<Department> Departments { get; set; }
        DbSet<Cargo> Cargos { get; set; }
        DbSet<CargoType> CargoTypes { get; set; }
        DbSet<CargoSection> CargoSections { get; set; }
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        Task<int> SaveAll();
        EntityEntry Entry(object entity);
    }
}
