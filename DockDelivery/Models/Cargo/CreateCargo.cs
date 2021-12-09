using System;
using System.ComponentModel.DataAnnotations;

namespace DockDelivery.Models.Cargo
{
    public class CreateCargo
    {
        [Required]
        public string CargoSectionId { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Owner { get; set; }

        [Required]
        public double Weight { get; set; }

        [Required]
        public double Capacity { get; set; }
    }
}
