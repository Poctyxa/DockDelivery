using System;

namespace DockDelivery.Domain.Entities
{
    public class Cargo : EntityBase
    {
        public string CargoSectionId { get; set; }
        public CargoSection CargoSection { get; set; }
        public string Description { get; set; }
        public string Owner { get; set; }
        public double Weight { get; set; }
        public double Capacity { get; set; }

    }
}
