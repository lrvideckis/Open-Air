using System;

namespace OpenAir.Data.Models
{
    public class Flight : IFlight
    {
        // class attributes
        public int FlightId { get; set; } // PK
        public DateTime TimeOfFlight { get; set; } 
        public float FlightSpeed { get; set; }

        // foreign key references
        public string PilotEmailId { get; set; }
        public string RouteId { get; set; }
        public Pilot Pilot { get; set; }
        public Route Route { get; set; }

    }
}
