using DockDelivery.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace DockDelivery.Domain.Context
{
    public class MongoDockDeliveryDbContext : IMongoDockDeliveryDbContext
    {
        private readonly IMongoDatabase db;

        public MongoDockDeliveryDbContext(IMongoDatabase db)
        {
            this.db = db;

            if (!BsonClassMap.IsClassMapRegistered(typeof(EntityBase)))
            {
                MapClasses();
            }
        }

        public IMongoCollection<Department> Departments {
            get
            {
                return db.GetCollection<Department>(typeof(Department).Name);
            }
        }
        public IMongoCollection<Cargo> Cargos
        {
            get
            {
                return db.GetCollection<Cargo>(typeof(Cargo).Name);
            }
        }
        public IMongoCollection<CargoType> CargoTypes
        {
            get
            {
                return db.GetCollection<CargoType>(typeof(CargoType).Name);
            }
        }
        public IMongoCollection<CargoSection> CargoSections
        {
            get
            {
                return db.GetCollection<CargoSection>(typeof(CargoSection).Name);
            }
        }

        private void MapClasses()
        {
            BsonClassMap.RegisterClassMap<EntityBase>(cm =>
            {
                cm.MapIdMember(c => c.Id)
                    .SetSerializer(new StringSerializer(BsonType.ObjectId));
                cm.SetIsRootClass(true);
                cm.AutoMap();
            });

            BsonClassMap.RegisterClassMap<Department>(cm =>
            { 
                cm.SetIgnoreExtraElements(true);

                cm.MapMember(c => c.DepartmentAddress).SetElementName("departmentAddress");
                cm.MapMember(c => c.DepartmentName).SetElementName("departmentName");
                cm.MapMember(c => c.LastSending).SetElementName("lastSending");
                cm.MapMember(c => c.NextSending).SetElementName("nextSending");

                cm.UnmapProperty(c => c.CargoSections); 
                cm.AutoMap();
            });

            BsonClassMap.RegisterClassMap<CargoSection>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);

                cm.MapMember(c => c.CargoTypeId).SetElementName("cargoTypeId");
                cm.MapMember(c => c.DepartmentId).SetElementName("departmentId");
                cm.MapMember(c => c.WeightLimit).SetElementName("weightLimit");
                cm.MapMember(c => c.CapacityLimit).SetElementName("capacityLimit");

                // Unmap property
                cm.UnmapProperty(c => c.Cargos);
            });

            BsonClassMap.RegisterClassMap<CargoType>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);

                cm.MapMember(c => c.TypeName).SetElementName("typeName");
            });

            BsonClassMap.RegisterClassMap<Cargo>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);

                cm.MapMember(c => c.Owner).SetElementName("owner");
                cm.MapMember(c => c.Capacity).SetElementName("capacity");
                cm.MapMember(c => c.Weight).SetElementName("weight");
                cm.MapMember(c => c.CargoSectionId).SetElementName("cargoSectionId");
                cm.MapMember(c => c.Description).SetElementName("description");

                // Unmap property
                cm.UnmapProperty(c => c.CargoSection);
            });
        }

    }
}
