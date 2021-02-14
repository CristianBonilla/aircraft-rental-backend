using System.Threading.Tasks;
using System.Collections.Generic;

namespace Rental.Domain
{
    public interface IAircraftService
    {
        Task<AircraftEntity> CreateAircraft(AircraftEntity aircraft);
        Task<AircraftEntity> AircraftById(int id);
        Task<AircraftEntity> UpdateAircraft(AircraftEntity aircraft);
        Task<AircraftEntity> DeleteAircraft(int id);
        IEnumerable<AircraftEntity> Aircrafts();
    }
}