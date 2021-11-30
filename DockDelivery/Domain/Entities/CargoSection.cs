using System;
using System.Collections.Generic;

namespace DockDelivery.Domain.Entities
{
    public class CargoSection : EntityBase
    {
        public Guid CargoTypeId { get; set; }
        public CargoType CargoType { get; set; }
        public Guid DepartmentId { get; set; }
        public Department Department { get; set; }
        public double CapacityLimit { get; set; }
        public double WeightLimit { get; set; }
        public virtual IEnumerable<Cargo> Cargos { get; set; }
    }
}
