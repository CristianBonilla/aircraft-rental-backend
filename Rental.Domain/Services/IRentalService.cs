using System.Threading.Tasks;
using System.Collections.Generic;

namespace Rental.Domain
{
    public interface IRentalService
    {
        Task<RentalEntity> CreateRental(RentalEntity rental);
        Task<PassengerEntity> CreatePassenger(PassengerEntity passenger);
        Task<RentalEntity> RentalById(int id);
        Task<PassengerEntity> PassengerById(int id);
        IAsyncEnumerable<RentalEntity> Rentals();
        IAsyncEnumerable<PassengerEntity> Passengers();
    }
}
