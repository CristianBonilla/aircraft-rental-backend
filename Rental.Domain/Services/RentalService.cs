using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Rental.Infrastructure;

namespace Rental.Domain
{
    public class RentalService : IRentalService
    {
        readonly IRepositoryContext<RentalContext> context;
        readonly IRepository<RentalContext, PassengerEntity> passengerRepository;
        readonly IRepository<RentalContext, RentalEntity> rentalRepository;

        public RentalService(
            IRepositoryContext<RentalContext> context,
            IRepository<RentalContext, PassengerEntity> passengerRepository,
            IRepository<RentalContext, RentalEntity> rentalRepository) =>
            (this.context, this.passengerRepository, this.rentalRepository) = (context, passengerRepository, rentalRepository);

        public async Task<PassengerEntity> CreatePassenger(PassengerEntity passenger)
        {
            PassengerEntity passengerCreated = passengerRepository.Create(passenger);
            _ = await context.SaveAsync();

            return passengerCreated;
        }

        public Task<PassengerEntity> PassengerById(int id)
        {
            PassengerEntity passenger = passengerRepository.Find(id);

            return Task.FromResult(passenger);
        }

        public IAsyncEnumerable<PassengerEntity> Passengers()
        {
            var passengers = passengerRepository.Get(orderBy: o => o.OrderBy(p => p.IdentificationDocument).ThenBy(p => p.FirstName))
                .ToAsyncEnumerable();

            return passengers;
        }

        public async Task<RentalEntity> CreateRental(RentalEntity rental)
        {
            RentalEntity rentalCreated = rentalRepository.Create(rental);
            _ = await context.SaveAsync();

            return rentalCreated;
        }

        public Task<RentalEntity> RentalById(int id)
        {
            RentalEntity rental = rentalRepository.Find(id);

            return Task.FromResult(rental);
        }

        public IAsyncEnumerable<RentalEntity> Rentals()
        {
            var rentals = rentalRepository.Get(orderBy: o => o.OrderBy(r => r.Location)).ToAsyncEnumerable();

            return rentals;
        }
    }
}
