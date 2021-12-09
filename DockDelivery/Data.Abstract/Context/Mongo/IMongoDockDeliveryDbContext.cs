using DockDelivery.Domain.Entities;
using MongoDB.Driver;

namespace DockDelivery.Domain.Context
{
    public interface IMongoDockDeliveryDbContext
    {
        IMongoCollection<Department> Departments { get; }
        IMongoCollection<Cargo> Cargos { get; }
        IMongoCollection<CargoType> CargoTypes { get; }
        IMongoCollection<CargoSection> CargoSections { get; }
    }
}
