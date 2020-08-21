using OpenAir.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenAir.Data
{
    public static class FlightDeconfliction
    {
        public static bool Deconflict(Flight flightToCheck, List<Flight> existingFlights)
        {
            for (int i = 0; i < existingFlights.Count; i++)
            {
                var timeDifference = flightToCheck.TimeOfFlight.Subtract(existingFlights[i].TimeOfFlight);
                var totalMinutes = Math.Abs(timeDifference.TotalMinutes);
                if (totalMinutes < 15.0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
