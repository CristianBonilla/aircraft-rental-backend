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
            CreateMap<UserRegisterRequest, UserEntity>()
                .ForMember(m => m.Id, m => m.Ignore())
                .ForMember(m => m.IdRole, m => m.Ignore())
                .ForMember(m => m.Role, m => m.Ignore())
                .ReverseMap()
                .ForMember(m => m.Role, m => m.Ignore());
        }
    }
}
