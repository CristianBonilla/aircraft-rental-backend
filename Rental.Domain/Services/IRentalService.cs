using System.Threading.Tasks;
using System.Collections.Generic;

namespace Rental.Domain
{
    public interface IRentalService
    {
        Task<PassengerEntity> CreatePassenger(PassengerEntity passenger);
        Task<PassengerEntity> PassengerById(int id);
        IAsyncEnumerable<PassengerEntity> Passengers();
        Task<RentalEntity> CreateRental(RentalEntity rental);
        Task<RentalEntity> RentalById(int id);
        IAsyncEnumerable<RentalEntity> Rentals();
    }
}
