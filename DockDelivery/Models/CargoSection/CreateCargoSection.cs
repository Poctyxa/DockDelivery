using System;
using System.ComponentModel.DataAnnotations;

namespace DockDelivery.Models.CargoSection
{
    public class CreateCargoSection
    {
        [Required]
        public string CargoTypeId { get; set; }

        [Required]
        public string DepartmentId { get; set; }

        [Required]
        public double CapacityLimit { get; set; }

        [Required]
        public double WeightLimit { get; set; }
    }
}
