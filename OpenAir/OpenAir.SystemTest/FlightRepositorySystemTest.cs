using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using OpenAir.Data;
using OpenAir.Data.Models;
using System.Linq;
using System.Threading.Tasks;

namespace OpenAir.SystemTest
{
    class FlightRepositorySystemTest
    {
        private OpenAirDbContext _context;
        private Pilot pilot;
        private Route route;

        [SetUp]
        public async Task Setup()
        {
            _context = SingletonTestSetup.Instance().Get();
            route = await _context.Routes.Where(r => r.RouteId == "VR-140").FirstOrDefaultAsync();
            pilot = new Pilot { Aircraft = "F-16", EmailId = "FlightTests@gmail.com", Name = "Test Pilot", Pwd = "a" };
            await _context.Pilots.AddAsync(pilot);
            _context.Entry(pilot).State = EntityState.Added;
            await _context.SaveChangesAsync();
        }
       
        public Flight CreateFlight(DateTime takeOff, string pilotEmail)
        {
            return new Flight { FlightSpeed = 200, PilotEmailId=pilotEmail, TimeOfFlight = takeOff, RouteId="VR-140"};
        }

        public Flight CreateFlight(DateTime takeOff)
        {
            return new Flight { FlightSpeed = 200, PilotEmailId = pilot.EmailId, TimeOfFlight = takeOff, RouteId = "VR-140", Pilot = pilot, Route = route };
        }

        public async Task AddForTest(Flight flight)
        {
            await _context.Flights.AddAsync(flight);
            _context.Entry(flight).State = EntityState.Added;
            await _context.SaveChangesAsync();
        }

        [Test]
        public async Task TestWhetherFlightGotAdded()
        {
            var repo = new FlightRepo(_context);
            var takeOff = new DateTime(2020, 5, 5, 8, 30, 0);
            var flight = CreateFlight(takeOff, "FlightTests@gmail.com");

            await repo.Add(flight);
            var result = await (_context.Flights.Where(f => f.FlightId == flight.FlightId)).FirstOrDefaultAsync();
            Assert.IsNotNull(result);

        }

        [Test]
        public async Task TestWhetherFlightGotDeleted()
        {
            var repo = new FlightRepo(_context);
            var takeOff = new DateTime(2020, 4, 4, 8, 30, 0);
            var flight = CreateFlight(takeOff, "FlightTests@gmail.com");

            await AddForTest(flight);
            await repo.Delete(flight.TimeOfFlight, "VR-140");
            var result = await (_context.Flights.Where(f => f.TimeOfFlight == flight.TimeOfFlight && f.RouteId == "VR-140")).FirstOrDefaultAsync();

            Assert.IsNull(result);
        }

        [Test]
        public async Task TestWhetherFlightWasModified()
        {
            var repo = new FlightRepo(_context);
            var takeOff = new DateTime(2020, 6, 6, 8, 30, 0);
            var flight = CreateFlight(takeOff, "FlightTests@gmail.com");
            var oldFlight = flight;

            await AddForTest(flight);
            flight.FlightSpeed = 500;
            await repo.Update(flight);

            var flights = await _context.Flights.ToListAsync();
            var foundFlight = flights.Where(f => f.FlightSpeed == flight.FlightSpeed).FirstOrDefault();

            Assert.IsTrue(foundFlight.FlightSpeed == flight.FlightSpeed && foundFlight.FlightId == oldFlight.FlightId);
        }

        [Test]
        public async Task TestWhetherAddFlightDeconflictThrowsSqlException()
        {
            // ARRANGE - setup repo, make a flight and 2 others-- one that conflicts and one that doesn't
            var repo = new FlightRepo(_context);
            var takeOff = new DateTime(2019, 3, 20, 9, 30, 0); // 9:30am, March 20th, 2019 - first flight
            var flight = CreateFlight(takeOff);
            var takeOffConflictBefore = new DateTime(2019, 3, 20, 9, 20, 0); // 9:20am, March 20th, 2019 - conflicting flight (before first flight)
            var flightConflictBefore = CreateFlight(takeOffConflictBefore);
            var takeOffConflictAfter = new DateTime(2019, 3, 20, 9, 40, 0); // 9:40am, March 20th, 2019 - conflicting flight (after first flight)
            var flightConflictAfter = CreateFlight(takeOffConflictBefore);
            var takeOffNoConflict = new DateTime(2018, 3, 20, 9, 30, 0); // 9:30am, March 20th, 2018 - non-conflicting flight
            var flightNoConflict = CreateFlight(takeOffNoConflict);

            // ACT - add the flights to the db
            try // add first flight
            {
                await repo.Add(flight);
            }
            catch (DbUpdateException) { Assert.IsTrue(false); } // manually throw Assertion Exception
            catch (InvalidOperationException) { Assert.IsTrue(false); } // this Add shouldn't create conflict

            try // add non-conflicting flight
            {
                await repo.Add(flightNoConflict);
            }
            catch (DbUpdateException) { Assert.IsTrue(false); } // manually throw Assertion Exception
            catch (InvalidOperationException) { Assert.IsTrue(false); } // this Add shouldn't create conflict


            try // add conflicting flight before, should throw a DbUpdateException upon Add attempt
            {
                await repo.Add(flightConflictBefore);
                Assert.IsTrue(false); // shouldn't get here
            }
            catch (DbUpdateException) { /* Should be caught here */ }
            catch (InvalidOperationException) { /* And here */ }


            try // add conflicting flight after, should throw a DbUpdateException upon Add attempt
            {
                await repo.Add(flightConflictAfter);
                Assert.IsTrue(false);
            }
            catch (DbUpdateException) { /* Should be caught here */ }
            catch (InvalidOperationException) { /* And here */ }

            // get the current list of flights to be sure the conflicting flight wasn't added
            var resultFlights = await _context.Flights.ToListAsync();

            // look for the conflicting flights in the db list -- result should be empty
            var resultBefore = resultFlights.Where(f => DateTime.Compare(f.TimeOfFlight, flightConflictBefore.TimeOfFlight) == 0
                                             && flightConflictBefore.PilotEmailId == f.PilotEmailId);
            var resultAfter = resultFlights.Where(f => DateTime.Compare(f.TimeOfFlight, flightConflictAfter.TimeOfFlight) == 0
                                             && flightConflictBefore.PilotEmailId == f.PilotEmailId);

            // ASSERT - should return no result
            Assert.IsEmpty(resultBefore);
            Assert.IsEmpty(resultAfter);
        }

        [TearDown]
        public async Task CleanUpFlightsAdded()
        {
            // get all flights in the db
            var flights = await _context.Flights.ToListAsync();

            // walk through and remove them
            foreach (Flight flight in flights)
            {
                _context.Remove(flight);
                _context.Entry(flight).State = EntityState.Deleted;
            }

            // save the changes in the db
            await _context.SaveChangesAsync();

            // get all pilots in the db
            var pilots = await _context.Pilots.ToListAsync();
            // walk through and remove them
            foreach (Pilot pilot in pilots)
            {
                _context.Remove(pilot);
                _context.Entry(pilot).State = EntityState.Deleted;
            }
            // save the changes in the db
            await _context.SaveChangesAsync();
        }
    }
}
