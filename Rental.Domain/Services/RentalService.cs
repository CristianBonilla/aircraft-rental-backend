using System;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;
using Rental.Infrastructure;

namespace Rental.Domain
{
    public class RentalService : IRentalService
    {
        readonly IRepositoryContext<RentalContext> context;
        readonly IRepository<RentalContext, PassengerEntity> passengerRepository;
        readonly IRepository<RentalContext, AircraftEntity> aircraftRepository;
        readonly IRepository<RentalContext, RentalEntity> rentalRepository;

        public RentalService(
            IRepositoryContext<RentalContext> context,
            IRepository<RentalContext, PassengerEntity> passengerRepository,
            IRepository<RentalContext, AircraftEntity> aircraftRepository,
            IRepository<RentalContext, RentalEntity> rentalRepository)
        {
            this.context = context;
            this.passengerRepository = passengerRepository;
            this.aircraftRepository = aircraftRepository;
            this.rentalRepository = rentalRepository;
        }

        public async IAsyncEnumerable<RentalEntity> CreateRental(RentalEntity rental, Guid[] passengerIDs)
        {
            var rentals = passengerIDs.Distinct().Where(passengerId => passengerRepository.Exists(p => p.Id == passengerId))
                .Select(passengerId => new RentalEntity
                {
                    PassengerId = passengerId,
                    AircraftId = rental.AircraftId,
                    Location = rental.Location,
                    ArrivalDate = rental.ArrivalDate,
                    DepartureDate = rental.DepartureDate
                }).ToList();
            var rentalsCreated = rentalRepository.CreateAll(rentals);
            AircraftEntity aircraft = aircraftRepository.Find(rental.AircraftId);
            aircraft.State = AircraftState.Rented;
            aircraftRepository.Update(aircraft);
            _ = await context.SaveAsync();
            await foreach (RentalEntity rentalCreated in rentalsCreated.ToAsyncEnumerable())
                yield return rentalCreated;
        }

        public async Task<PassengerEntity> CreatePassenger(PassengerEntity passenger)
        {
            PassengerEntity passengerCreated = passengerRepository.Create(passenger);
            _ = await context.SaveAsync();

            return passengerCreated;
        }

        public Task<RentalEntity> FindRental(Expression<Func<RentalEntity, bool>> expression)
        {
            RentalEntity rental = rentalRepository.Find(expression);

            return Task.FromResult(rental);
        }

        public Task<PassengerEntity> FindPassenger(Expression<Func<PassengerEntity, bool>> expression)
        {
            PassengerEntity passenger = passengerRepository.Find(expression);

            return Task.FromResult(passenger);
        }

        public IAsyncEnumerable<RentalEntity> Rentals()
        {
            var rentals = rentalRepository.Get(orderBy: o => o.OrderBy(r => r.Location)).ToAsyncEnumerable();

            return rentals;
        }

        public IAsyncEnumerable<PassengerEntity> Passengers()
        {
            var passengers = passengerRepository.Get(orderBy: o => o.OrderBy(p => p.IdentificationDocument).ThenBy(p => p.FirstName))
                .ToAsyncEnumerable();

            return passengers;
        }
    }
}
