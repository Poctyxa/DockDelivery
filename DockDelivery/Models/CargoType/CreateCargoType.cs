using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DockDelivery.Models.CargoType
{
    public class CreateCargoType
    {
        [Required]
        public string TypeName { get; set; }
    }
}
