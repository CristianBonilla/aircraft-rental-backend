using System.Collections.Generic;

namespace Rental.API
{
    public class AuthFailedResponse
    {
        public IEnumerable<string> Errors { get; set; }
    }
}
