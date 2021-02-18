using System;

namespace Rental.API
{
    public class RentalRequest
    {
        public int PassengerId { get; set; }
        public int AircraftId { get; set; }
        public string Location { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureDate { get; set; }
    }
}
