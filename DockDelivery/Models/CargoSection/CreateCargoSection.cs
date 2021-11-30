using System;
using System.ComponentModel.DataAnnotations;

namespace DockDelivery.Models.CargoSection
{
    public class CreateCargoSection
    {
        [Required]
        public Guid CargoTypeId { get; set; }

        [Required]
        public Guid DepartmentId { get; set; }

        [Required]
        public double CapacityLimit { get; set; }

        [Required]
        public double WeightLimit { get; set; }
    }
}
