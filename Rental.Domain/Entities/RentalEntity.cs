using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Rental.Domain
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum AircraftState
    {
        [EnumMember(Value = "N")]
        NotRented = 'N',
        [EnumMember(Value = "R")]
        Rented = 'R'
    }

    public class AircraftEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public AircraftState State { get; set; }
        public string Description { get; set; }
    }

    public class RentalEntity
    {
        public Guid Id { get; set; }
        public Guid PassengerId { get; set; }
        public Guid AircraftId { get; set; }
        public string Location { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureDate { get; set; }
        public PassengerEntity Passenger { get; set; }
        public AircraftEntity Aircraft { get; set; }
    }

    public class PassengerEntity
    {
        public Guid Id { get; set; }
        public long IdentificationDocument { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Specialty { get; set; }
    }
}
