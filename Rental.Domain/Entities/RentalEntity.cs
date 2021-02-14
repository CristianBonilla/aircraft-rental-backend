using System;

namespace Rental.Domain
{
    public enum AircraftState
    {
        NotRented = 'N',
        Rented = 'R'
    }

    public class AircraftEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AircraftState State { get; set; }
        public string Description { get; set; }
    }

    public class RentalEntity
    {
        public int Id { get; set; }
        public int IdPassenger { get; set; }
        public int IdAircraft { get; set; }
        public string Location { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureDate { get; set; }
        public PassengerEntity Passenger { get; set; }
        public AircraftEntity Aircraft { get; set; }
    }

    public class PassengerEntity
    {
        public int Id { get; set; }
        public long IdentificationDocument { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Specialty { get; set; }
    }
}
