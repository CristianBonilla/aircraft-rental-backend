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
            CreateMap<UserResponse, UserEntity>()
                .ForMember(m => m.Role, m => m.Ignore())
                .ReverseMap();

            CreateMap<RentalRequest, RentalEntity>()
                .ForMember(m => m.Id, m => m.Ignore())
                .ForMember(m => m.Aircraft, m => m.Ignore())
                .ForMember(m => m.Passenger, m => m.Ignore())
                .ForMember(m => m.PassengerId, m => m.Ignore())
                .ReverseMap()
                .ForMember(m => m.PassengerIDs, m => m.Ignore());

            CreateMap<RoleRequest, RoleEntity>()
                .ForMember(m => m.Id, m => m.Ignore())
                .ForMember(m => m.Permissions, m => m.Ignore())
                .ReverseMap();
            CreateMap<RoleResponse, RoleEntity>()
                .ForMember(m => m.Permissions, m => m.Ignore())
                .ReverseMap();

            CreateMap<PassengerRequest, PassengerEntity>()
                .ForMember(m => m.Id, m => m.Ignore())
                .ReverseMap();
        }
    }
}
