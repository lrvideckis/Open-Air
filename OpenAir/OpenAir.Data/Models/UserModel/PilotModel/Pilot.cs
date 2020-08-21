using System.Collections.Generic;

namespace OpenAir.Data.Models
{
    public class Pilot : IUser
    {
        // class attributes
        public string EmailId { get; set; } // PK
        public string Name { get; set; }
        public string Pwd { get; set; }
        public string Aircraft { get; set; }
        public bool IsAdmin { get; set; } = false;

        // Pilot has zero to many flights
        public IEnumerable<Flight> Flights { get; }

    }
}
