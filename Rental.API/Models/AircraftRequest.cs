using Rental.Domain;

namespace Rental.API
{
    public class AircraftRequest
    {
        public string Name { get; set; }
        public AircraftState State { get; set; }
        public string Description { get; set; }
    }
}
