using System;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace Rental.Domain
{
    public interface IAircraftService
    {
        Task<AircraftEntity> CreateAircraft(AircraftEntity aircraft);
        Task<AircraftEntity> FindAircraft(Expression<Func<AircraftEntity, bool>> expression);
        Task<AircraftEntity> UpdateAircraft(AircraftEntity aircraft);
        Task<AircraftEntity> DeleteAircraft(AircraftEntity aircraft);
        IAsyncEnumerable<AircraftEntity> Aircrafts();
        IAsyncEnumerable<AircraftEntity> AircraftsByState(AircraftState state);
    }
}
