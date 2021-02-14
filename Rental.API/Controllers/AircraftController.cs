using Microsoft.AspNetCore.Mvc;
using Rental.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rental.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AircraftController : ControllerBase
    {
        readonly IAircraftService aircraftService;

        public AircraftController(IAircraftService aircraftService) =>
            (this.aircraftService) = (aircraftService);

        // GET: api/<AircraftController>
        [HttpGet]
        public async IAsyncEnumerable<AircraftEntity> Get()
        {
            var aircrafts = aircraftService.Aircrafts();
            await foreach (AircraftEntity aircraft in aircrafts)
                yield return aircraft;
        }

        // GET api/<AircraftController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<AircraftController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<AircraftController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AircraftController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
