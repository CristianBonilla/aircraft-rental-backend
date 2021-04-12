using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;

namespace Rental.Domain
{
    public interface IRentalService
    {
        IAsyncEnumerable<RentalEntity> CreateRental(RentalEntity rental, Guid[] passengerIDs);
        Task<PassengerEntity> CreatePassenger(PassengerEntity passenger);
        Task<RentalEntity> FindRental(Expression<Func<RentalEntity, bool>> expression);
        Task<PassengerEntity> FindPassenger(Expression<Func<PassengerEntity, bool>> expression);
        IAsyncEnumerable<RentalEntity> Rentals();
        IAsyncEnumerable<PassengerEntity> Passengers();
    }
}
