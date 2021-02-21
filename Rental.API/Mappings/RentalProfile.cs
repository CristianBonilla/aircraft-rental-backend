using AutoMapper;
using Rental.Domain;

namespace Rental.API
{
    public class RentalProfile : Profile
    {
        public RentalProfile()
        {
            CreateMap<AircraftRequest, AircraftEntity>()
                .ForMember(m => m.Id, m => m.Ignore())
                .ReverseMap();
            CreateMap<UserRegisterRequest, UserEntity>()
                .ForMember(m => m.Id, m => m.Ignore())
                .ForMember(m => m.RoleId, m => m.Ignore())
                .ForMember(m => m.Role, m => m.Ignore())
                .ReverseMap()
                .ForMember(m => m.Role, m => m.Ignore());
            CreateMap<RentalRequest, RentalEntity>()
                .ForMember(m => m.Id, m => m.Ignore())
                .ForMember(m => m.Aircraft, m => m.Ignore())
                .ForMember(m => m.Passenger, m => m.Ignore())
                .ReverseMap();
            CreateMap<RoleRequest, RoleEntity>()
                .ForMember(m => m.Id, m => m.Ignore())
                .ForMember(m => m.Permissions, m => m.Ignore())
                .ReverseMap();
            CreateMap<PassengerRequest, PassengerEntity>()
                .ForMember(m => m.Id, m => m.Ignore())
                .ReverseMap();
        }
    }
}
