using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Rental.Domain;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Rental.API.Controllers.V1
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AircraftController : ControllerBase
    {
        readonly IMapper mapper;
        readonly IAircraftService aircraftService;

        public AircraftController(IMapper mapper, IAircraftService aircraftService) =>
            (this.mapper, this.aircraftService) = (mapper, aircraftService);

        [HttpGet(ApiRoutes.Aircraft.Get)]
        public async IAsyncEnumerable<AircraftEntity> Get()
        {
            var aircrafts = aircraftService.Aircrafts();
            await foreach (AircraftEntity aircraft in aircrafts)
                yield return aircraft;
        }

        [HttpGet(ApiRoutes.Aircraft.GetById)]
        public async Task<IActionResult> Get(int id)
        {
            AircraftEntity aircraft = await aircraftService.AircraftById(id);
            if (aircraft == null)
                return NotFound();

            return Ok(aircraft);
        }

        [HttpPost(ApiRoutes.Aircraft.Create)]
        public async Task<IActionResult> Post([FromBody] AircraftEntity aircraft)
        {
            AircraftEntity aircraftCreated = await aircraftService.CreateAircraft(aircraft);

            return Ok(aircraftCreated);
        }

        [HttpPut(ApiRoutes.Aircraft.Update)]
        public async Task<IActionResult> Put(int id, [FromBody] AircraftEntity aircraft)
        {
            AircraftEntity aircraftFound = await aircraftService.AircraftById(id);
            if (aircraftFound == null)
                return NotFound();
            AircraftEntity aircraftMap = mapper.Map(aircraft, aircraftFound);
            AircraftEntity aircraftUpdated = await aircraftService.UpdateAircraft(aircraftMap);

            return Ok(aircraftUpdated);
        }

        [HttpDelete(ApiRoutes.Aircraft.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            AircraftEntity aircraftFound = await aircraftService.AircraftById(id);
            if (aircraftFound == null)
                return NotFound();
            AircraftEntity aircraftDeleted = await aircraftService.DeleteAircraft(aircraftFound);

            return Ok(aircraftDeleted);
        }
    }
}
