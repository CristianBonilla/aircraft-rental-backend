using System.Collections.Generic;

namespace Rental.API.Models
{
    public class AuthFailedResponse
    {
        public IEnumerable<string> Errors { get; set; }
    }
}
