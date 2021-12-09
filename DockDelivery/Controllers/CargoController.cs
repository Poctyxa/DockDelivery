using DockDelivery.Business.Abstract;
using DockDelivery.Domain.Entities;
using DockDelivery.Models.Cargo;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DockDelivery.Controllers
{
    [Route("api/[controller]")]
    public class CargoController : Controller
    {
        private readonly ICargoService cargoService;

        public CargoController(ICargoService cargoService)
        {
            this.cargoService = cargoService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var cargos = await cargoService.GetCargosAsync();
            return Ok(cargos);
        }

        [HttpGet]
        [Route("{cargoId}")]
        public IActionResult Read(string cargoId)
        {
            var cargo = cargoService.GetByIdAsync(cargoId).Result;
            return Ok(cargo);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCargo cargo)
        {
            if (ModelState.IsValid)
            {
                var newCargo = new Cargo();
                newCargo.CargoSectionId = cargo.CargoSectionId;
                newCargo.Owner = cargo.Owner;
                newCargo.Capacity = cargo.Capacity;
                newCargo.Weight = cargo.Weight;
                newCargo.Description = cargo.Description;

                await cargoService.CreateAsync(newCargo);

                if (newCargo.Id.Length > 0)
                    return Ok("Cargo was added");
            }

            return BadRequest("Cargo was not added");
        }

        [HttpPatch]
        public async Task<IActionResult> Update([FromBody] Cargo cargo)
        {
            if (ModelState.IsValid)
            {
                await cargoService.UpdateAsync(cargo);
                return Ok("Cargo was updated");
            }

            return BadRequest("Cargo was not updated");
        }


        [HttpDelete]
        [Route("{cargoId}")]
        public async Task<IActionResult> Remove(string cargoId)
        {
            if (ModelState.IsValid)
            {
                await cargoService.RemoveAsync(cargoId);
                return Ok("Cargo was removed");
            }

            return BadRequest("Cargo was not removed");
        }
    }
}
