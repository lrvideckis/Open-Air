using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenAir.Data.Models;
using System.Data.SqlClient;

namespace OpenAir.Data
{
    public class FlightRepo : IFlightRepo
    {
        private readonly OpenAirDbContext db;

        /*
         * Class constructor creates db context for flights
         */
        public FlightRepo(OpenAirDbContext db)
        {
            this.db = db;
        }

        /*
         * Asynchronus add to flight database
         */
        public async Task Add(IFlight flight)
        {
            // case IFlight to flight, get current flight list from db
            Flight f = (Flight)flight;
            var flights = await db.Flights.ToListAsync();

            // check if flight conflicts, if so throw SqlException
            if(!FlightDeconfliction.Deconflict(f, flights)) 
            {
                // throw sql exception and return... no way to manually throw SqlException in C#
                SqlConnection connection = new SqlConnection("Server=(localdb)\\mssqllocaldb;Database=OpenAirLocalTestDb;Trusted_Connection=True;MultipleActiveResultSets=true");
                SqlCommand cmd =
                new SqlCommand("raiserror('Manual SQL exception', 16, 1)", connection);
                cmd.ExecuteNonQuery();
                return;
            }
            // no conflict so add the new flight
            await db.Flights.AddAsync((Flight)flight);
            await db.SaveChangesAsync();
        }

        /*
         * Asynchronus delete from flight database
         * will catch if flight could not be found in table
         */
        public async Task Delete(DateTime takeOff, string routeId)
        {
            try
            {
                var flight = await GetFlight(takeOff, routeId);
                db.Flights.Remove((Flight)flight);
                db.Entry(flight).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                await db.SaveChangesAsync();
            }
            catch (ArgumentNullException) { };
        }

        /*
         *  Gets all flights from database and returns enumerable list
         */
        public async Task<IEnumerable<IFlight>> GetAll()
        {
            var flights = await db.Flights.ToListAsync();
            return flights.AsEnumerable();
        }

        /*
         * Returns all flights connected to a specific pilot
         * returns enumerable list of flights
         */
        public async Task<IEnumerable<IFlight>> GetAll(string email)//for my flights view
        {
            var results = await db.Flights.ToListAsync();
            return results.Where(f => f.PilotEmailId == email).OrderBy(f => f.TimeOfFlight).ToList().AsEnumerable();
        }

        public async Task<IEnumerable<IFlight>> GetAll(DateTime date)//for schedule flights view
        {
            var results = await db.Flights.ToListAsync();
            return results.Where(f => f.TimeOfFlight.DayOfYear == date.DayOfYear).OrderBy(f => f.TimeOfFlight).ToList().AsEnumerable();
        }

        /*
         * Gets all flights on a specific route on a certain date for a specific pilot
         * returns enumerable list of those flights
         */
        public async Task<IEnumerable<IFlight>> GetAll(DateTime date, string route, string email)//for schedule flights view
        {
            var results = await db.Flights.ToListAsync();
            return results.Where(f => f.TimeOfFlight.Date == date.Date && 
                                      f.RouteId == route  && 
                                      f.PilotEmailId == email).OrderBy(f => f.TimeOfFlight).ToList().AsEnumerable();
        }

        /*
         * Gets a specific flight based on ID
         */
        public async Task<IFlight> GetFlight(DateTime takeOff, string routeId)
        {
            var result =  await db.Flights.Where(f => f.TimeOfFlight == takeOff && f.RouteId == routeId).FirstOrDefaultAsync();
            return result;
        }

        /*
         * updates flight in database
         */
        public async Task Update(IFlight flight)
        {
            var entry = db.Entry(flight);
            entry.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await db.SaveChangesAsync();
        }
    }
}
