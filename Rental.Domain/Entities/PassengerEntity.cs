namespace Rental.Domain
{
    public class PassengerEntity
    {
        public int Id { get; set; }
        public long IdentificationDocument { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Specialty { get; set; }
    }
}
