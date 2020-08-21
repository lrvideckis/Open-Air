using System.Collections.Generic;

namespace OpenAir.Data.Models
{
    public class Route : IRoute
    {
        // primary key - class attribute
        public string RouteId { get; set; }

        // Route has zero to many Flights and one to many RoutePoints
        public IEnumerable<Flight> Flights { get; set; }
        public IEnumerable<RoutePoint> RoutePoints  { get; set; }
    }
}
