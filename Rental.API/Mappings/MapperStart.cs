using AutoMapper;

namespace Rental.API
{
    public class MapperStart
    {
        public static MapperConfiguration Start()
        {
            MapperConfiguration config = new MapperConfiguration(c => c.AddProfile<RentalProfile>());
            config.AssertConfigurationIsValid<RentalProfile>();

            return config;
        }
    }
}
