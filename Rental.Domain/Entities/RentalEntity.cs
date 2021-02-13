using System;

namespace Rental.Domain
{
    public class RentalEntity
    {
        public int Id { get; set; }
        public int IdPassenger { get; set; }
        public string Location { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureDate { get; set; }
        public PassengerEntity Passenger { get; set; }
    }
}
