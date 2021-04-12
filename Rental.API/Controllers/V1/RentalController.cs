using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Rental.Domain;

namespace Rental.API.Controllers.V1
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RentalController : ControllerBase
    {
        readonly IMapper mapper;
        readonly IRentalService rentalService;

        public RentalController(IMapper mapper, IRentalService rentalService) =>
            (this.mapper, this.rentalService) = (mapper, rentalService);

        [Authorize(Policy = "RentalsPolicy")]
        [HttpPost(ApiRoutes.V1.Rental.CreateRental)]
        public async IAsyncEnumerable<RentalEntity> CreateRental([FromBody] RentalRequest rentalRequest)
        {
            RentalEntity rentalMap = mapper.Map<RentalEntity>(rentalRequest);
            var rentalsCreated = rentalService.CreateRental(rentalMap, rentalRequest.PassengerIDs);
            await foreach (RentalEntity rental in rentalsCreated)
                yield return rental;
        }

        [Authorize(Policy = "PassengersPolicy")]
        [HttpPost(ApiRoutes.V1.Rental.CreatePassenger)]
        public async Task<IActionResult> CreatePassenger([FromBody] PassengerRequest passengerRequest)
        {
            PassengerEntity passenger = mapper.Map<PassengerEntity>(passengerRequest);
            PassengerEntity passengerCreated = await rentalService.CreatePassenger(passenger);

            return Ok(passengerCreated);
        }

        [Authorize(Policy = "RentalsPolicy")]
        [HttpGet(ApiRoutes.V1.Rental.GetRentalById)]
        public async Task<IActionResult> GetRentalById(Guid id)
        {
            RentalEntity rental = await rentalService.FindRental(r => r.Id == id);
            if (rental == null)
                return NotFound();

            return Ok(rental);
        }

        [Authorize(Policy = "PassengersPolicy")]
        [HttpGet(ApiRoutes.V1.Rental.GetPassengerById)]
        public async Task<IActionResult> GetPassengerById(Guid id)
        {
            PassengerEntity passenger = await rentalService.FindPassenger(p => p.Id == id);
            if (passenger == null)
                return NotFound();

            return Ok(passenger);
        }

        [Authorize(Policy = "RentalsPolicy")]
        [HttpGet(ApiRoutes.V1.Rental.GetRentals)]
        public async IAsyncEnumerable<RentalEntity> GetRentals()
        {
            var rentals = rentalService.Rentals();
            await foreach (RentalEntity rental in rentals)
                yield return rental;
        }

        [Authorize(Policy = "RentalsPolicy")]
        [HttpGet(ApiRoutes.V1.Rental.GetRentalsByAircraftId)]
        public async IAsyncEnumerable<RentalEntity> GetRentalsByAircraftId(Guid aircraftId)
        {
            var rentalsd = await rentalService.RentalsByAircraftId(aircraftId).ToListAsync();
            var rentals = rentalService.RentalsByAircraftId(aircraftId);
            await foreach (RentalEntity rental in rentals)
                yield return rental;
        }

        [Authorize(Policy = "PassengersPolicy")]
        [HttpGet(ApiRoutes.V1.Rental.GetPassengers)]
        public async IAsyncEnumerable<PassengerEntity> GetPassengers()
        {
            var passengers = rentalService.Passengers();
            await foreach (PassengerEntity passenger in passengers)
                yield return passenger;
        }

        [Authorize(Policy = "RentalsPolicy")]
        [HttpGet(ApiRoutes.V1.Rental.GetPassengersNotAvailable)]
        public async IAsyncEnumerable<PassengerEntity> GetPassengersNotAvailable()
        {
            var passengers = rentalService.Passengers();
            var rentals = rentalService.Rentals();
            await foreach (PassengerEntity passenger in passengers)
                if (!await rentals.AnyAsync(rental => rental.PassengerId == passenger.Id))
                    yield return passenger;
        }
    }
}
