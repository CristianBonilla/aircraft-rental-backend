using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Rental.Domain;

namespace Rental.API.Controllers.V1
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RentalController : ControllerBase
    {
        readonly IRentalService rentalService;

        public RentalController(IRentalService rentalService) =>
            (this.rentalService) = (rentalService);

        [HttpPost(ApiRoutes.Rental.CreateRental)]
        public async Task<IActionResult> CreateRental([FromBody] RentalEntity rental)
        {
            RentalEntity rentalCreated = await rentalService.CreateRental(rental);

            return Ok(rentalCreated);
        }

        [HttpPost(ApiRoutes.Rental.CreatePassenger)]
        public async Task<IActionResult> CreatePassenger([FromBody] PassengerEntity passenger)
        {
            PassengerEntity passengerCreated = await rentalService.CreatePassenger(passenger);

            return Ok(passengerCreated);
        }

        [HttpGet(ApiRoutes.Rental.GetRentalById)]
        public async Task<IActionResult> GetRentalById(int id)
        {
            RentalEntity rental = await rentalService.FindRental(r => r.Id == id);
            if (rental == null)
                return NotFound();

            return Ok(rental);
        }

        [HttpGet(ApiRoutes.Rental.GetPassengerById)]
        public async Task<IActionResult> GetPassengerById(int id)
        {
            PassengerEntity passenger = await rentalService.FindPassenger(p => p.Id == id);
            if (passenger == null)
                return NotFound();

            return Ok(passenger);
        }

        [HttpGet(ApiRoutes.Rental.GetRentals)]
        public async IAsyncEnumerable<RentalEntity> GetRentals()
        {
            var rentals = rentalService.Rentals();
            await foreach (RentalEntity rental in rentals)
                yield return rental;
        }

        [HttpGet(ApiRoutes.Rental.GetPassengers)]
        public async IAsyncEnumerable<PassengerEntity> GetPassengers()
        {
            var passengers = rentalService.Passengers();
            await foreach (PassengerEntity passenger in passengers)
                yield return passenger;
        }
    }
}
