using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Rental.Infrastructure;

namespace Rental.Domain
{
    public class AircraftService : IAircraftService
    {
        readonly IRepositoryContext<RentalContext> context;
        readonly IRepository<RentalContext, AircraftEntity> repository;

        public AircraftService(
            IRepositoryContext<RentalContext> context,
            IRepository<RentalContext, AircraftEntity> repository) =>
            (this.context, this.repository) = (context, repository);

        public async Task<AircraftEntity> CreateAircraft(AircraftEntity aircraft)
        {
            AircraftEntity aircraftCreated = repository.Create(aircraft);
            await context.SaveAsync();

            return aircraftCreated;
        }

        public Task<AircraftEntity> AircraftById(int id)
        {
            AircraftEntity aircraft = repository.Find(id);

            return Task.FromResult(aircraft);
        }

        public async Task<AircraftEntity> UpdateAircraft(AircraftEntity aircraft)
        {
            AircraftEntity aircraftUpdated = repository.Update(aircraft);
            await context.SaveAsync();

            return aircraftUpdated;
        }

        public async Task<AircraftEntity> DeleteAircraft(int id)
        {
            AircraftEntity aircraft = await AircraftById(id);
            if (aircraft != null)
            {
                repository.Delete(aircraft);
                await context.SaveAsync();
            }

            return aircraft;
        }

        public IEnumerable<AircraftEntity> Aircrafts() => repository.Get(orderBy: o => o.OrderBy(a => a.Name));
    }
}
