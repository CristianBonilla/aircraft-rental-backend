using AutoMapper;
using Rental.Domain;

namespace Rental.API
{
    public class RentalProfile : Profile
    {
        public RentalProfile()
        {
            CreateMap<AircraftEntity, AircraftEntity>()
                .ReverseMap();
        }
    }
}
