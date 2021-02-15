using System;

namespace Rental.API
{
    public class RentalRequest
    {
        public int IdPassenger { get; set; }
        public int IdAircraft { get; set; }
        public string Location { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureDate { get; set; }
    }
}
