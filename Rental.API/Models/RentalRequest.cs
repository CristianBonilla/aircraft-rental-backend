using System;

namespace Rental.API
{
    public class RentalRequest
    {
        public Guid PassengerId { get; set; }
        public Guid AircraftId { get; set; }
        public string Location { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureDate { get; set; }
    }
}
