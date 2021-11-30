using DockDelivery.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DockDelivery.Domain.Context
{
    public class DockDeliveryDbContext : DbContext, IDockDeliveryDbContext
    {
        public DockDeliveryDbContext(DbContextOptions<DockDeliveryDbContext> options) : base(options)
        {
        }

        public DbSet<Department> Departments { get ; set; }
        public DbSet<Cargo> Cargos { get; set; }
        public DbSet<CargoType> CargoTypes { get; set; }
        public DbSet<CargoSection> CargoSections { get; set; }

        public async Task<int> SaveAll()
        {
            return await this.SaveChangesAsync();
        }
    }
}
