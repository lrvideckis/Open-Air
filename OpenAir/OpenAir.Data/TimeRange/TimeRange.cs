using OpenAir.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

using System.Threading.Tasks;
using System.Linq;

namespace OpenAir.Data.TimeRange
{
    public static class TimeRange
    {

        /*Will Return all flights found between two dates
         * Times in between flights are all available time slots
         */
        public static async Task<List<DateTime>>getAvailableTimes(FlightRepo _db_flights, DateTime start, DateTime end)
        {
            DateTime current = start;
            List<DateTime> allFlightTimes = new List<DateTime>();
            //Get all flights for each day between two time periods
            while (current.DayOfYear <= end.DayOfYear )
            {
                var dayFlights = await _db_flights.GetAll(current);
                foreach (var flight in dayFlights)
                {
                    allFlightTimes.Append(flight.TimeOfFlight);
                }
                current = current.AddDays(1);
            }

            //return allFlightTimes; Could return all unavailable times if better for front end

            /*
             * Assuming that flights are schedule rounded to the 
             * quarter hour
             */
            List<DateTime> allAvailableTimes = new List<DateTime>();
            current = start;
            while (current.DayOfYear <= end.DayOfYear)
            {
                //When current time passes earliest time, earliest time
                //is removed from list
                if (current.TimeOfDay > allFlightTimes[0].TimeOfDay)
                {
                    allFlightTimes.RemoveRange(0, 1);
                }

                //Add current time to available times if it does not
                //conflict with next earliest time
                if (current.TimeOfDay != allFlightTimes[0].TimeOfDay)
                {
                    allAvailableTimes.Add(current);
                }

                current = current.AddMinutes(15);
            }

            return allAvailableTimes;
            
        }
    }
}
