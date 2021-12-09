using DockDelivery.Business.Abstract;
using DockDelivery.Domain.Entities;
using DockDelivery.Models.CargoType;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DockDelivery.Controllers
{
    [Route("api/[controller]")]
    public class CargoTypeController : Controller
    {
        private readonly ICargoTypeService cargoTypeService;

        public CargoTypeController(ICargoTypeService cargoTypeService)
        {
            this.cargoTypeService = cargoTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var cargoTypes = await cargoTypeService.GetCargoTypesAsync();
            return Ok(cargoTypes);
        }

        [HttpGet]
        [Route("{cargoTypeId}")]
        public IActionResult Read(string cargoTypeId)
        {
            var cargoType = cargoTypeService.GetByIdAsync(cargoTypeId).Result;
            return Ok(cargoType);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCargoType cargoType)
        {
            if (ModelState.IsValid)
            {
                var newCargoType = new CargoType();
                newCargoType.TypeName = cargoType.TypeName;

                await cargoTypeService.CreateAsync(newCargoType);

                if (newCargoType.Id.Length > 0)
                    return Ok("CargoSection was added");
            }

            return BadRequest("CargoSection was not added");
        }

        [HttpPatch]
        public async Task<IActionResult> Update([FromBody] CargoType cargoType)
        {
            if (ModelState.IsValid)
            {
                await cargoTypeService.UpdateAsync(cargoType);
                return Ok("CargoType was updated");
            }

            return BadRequest("CargoType was not updated");
        }


        [HttpDelete]
        [Route("{cargoTypeId}")]
        public async Task<IActionResult> Remove(string cargoTypeId)
        {
            if (ModelState.IsValid)
            {
                await cargoTypeService.RemoveAsync(cargoTypeId);
                return Ok("CargoType was removed");
            }

            return BadRequest("CargoType was not removed");
        }
    }
}
