using System;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
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
            aircraft.State = AircraftState.NotRented;
            AircraftEntity aircraftCreated = repository.Create(aircraft);
            _ = await context.SaveAsync();

            return aircraftCreated;
        }

        public Task<AircraftEntity> FindAircraft(Expression<Func<AircraftEntity, bool>> expression)
        {
            AircraftEntity aircraftFound = repository.Find(expression);

            return Task.FromResult(aircraftFound);
        }

        public async Task<AircraftEntity> UpdateAircraft(AircraftEntity aircraft)
        {
            AircraftEntity aircraftUpdated = repository.Update(aircraft);
            _ = await context.SaveAsync();

            return aircraftUpdated;
        }

        public async Task<AircraftEntity> DeleteAircraft(AircraftEntity aircraft)
        {
            AircraftEntity aircraftDeleted = repository.Delete(aircraft);
            _ = await context.SaveAsync();

            return aircraftDeleted;
        }

        public IAsyncEnumerable<AircraftEntity> Aircrafts()
        {
            var aircrafts = repository.Get(orderBy: o => o.OrderBy(a => a.Name)).ToAsyncEnumerable();

            return aircrafts;
        }

        public IAsyncEnumerable<AircraftEntity> AircraftsByState(AircraftState state)
        {
            var aircrafts = repository.Get(f => f.State == state, o => o.OrderBy(a => a.Name)).ToAsyncEnumerable();

            return aircrafts;
        }
    }
}
