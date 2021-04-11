using System;
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

        [Authorize(Policy = "AircraftsPolicy")]
        [HttpGet(ApiRoutes.V1.Aircraft.Get)]
        public async IAsyncEnumerable<AircraftEntity> Get()
        {
            var aircrafts = aircraftService.Aircrafts();
            await foreach (AircraftEntity aircraft in aircrafts)
                yield return aircraft;
        }

        [Authorize(Policy = "AircraftsPolicy")]
        [HttpGet(ApiRoutes.V1.Aircraft.GetById)]
        public async Task<IActionResult> Get(Guid id)
        {
            AircraftEntity aircraft = await aircraftService.FindAircraft(a => a.Id == id);
            if (aircraft == null)
                return NotFound();

            return Ok(aircraft);
        }

        [Authorize(Policy = "RentalsPolicy")]
        [HttpGet(ApiRoutes.V1.Aircraft.GetByState)]
        public async IAsyncEnumerable<AircraftEntity> GetByState(AircraftState state)
        {
            var aircrafts = aircraftService.AircraftsByState(state);
            await foreach (AircraftEntity aircraft in aircrafts)
                yield return aircraft;
        }

        [Authorize(Policy = "AircraftsPolicy")]
        [HttpPost(ApiRoutes.V1.Aircraft.Create)]
        public async Task<IActionResult> Post([FromBody] AircraftRequest aircraftRequest)
        {
            AircraftEntity aircraft = mapper.Map<AircraftEntity>(aircraftRequest);
            AircraftEntity aircraftCreated = await aircraftService.CreateAircraft(aircraft);

            return Ok(aircraftCreated);
        }

        [Authorize(Policy = "AircraftsPolicy")]
        [HttpPut(ApiRoutes.V1.Aircraft.Update)]
        public async Task<IActionResult> Put(Guid id, [FromBody] AircraftRequest aircraftRequest)
        {
            AircraftEntity aircraft = await aircraftService.FindAircraft(a => a.Id == id);
            if (aircraft == null)
                return NotFound();
            AircraftEntity aircraftMap = mapper.Map(aircraftRequest, aircraft);
            AircraftEntity aircraftUpdated = await aircraftService.UpdateAircraft(aircraftMap);

            return Ok(aircraftUpdated);
        }

        [Authorize(Policy = "AircraftsPolicy")]
        [HttpDelete(ApiRoutes.V1.Aircraft.Delete)]
        public async Task<IActionResult> Delete(Guid id)
        {
            AircraftEntity aircraftFound = await aircraftService.FindAircraft(a => a.Id == id);
            if (aircraftFound == null)
                return NotFound();
            _ = await aircraftService.DeleteAircraft(aircraftFound);

            return NoContent();
        }
    }
}
