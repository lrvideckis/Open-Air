using System;

namespace OpenAir.Data.Models
{
    public interface IFlight
    {
        public int FlightId { get; set; }

        public DateTime TimeOfFlight { get; set; }

        public string PilotEmailId { get; set; }

        public string RouteId { get; set; }

        public float FlightSpeed { get; set; }
    }
}
