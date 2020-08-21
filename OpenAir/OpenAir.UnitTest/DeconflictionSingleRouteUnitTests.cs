using NUnit.Framework;
using OpenAir.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenAir.UnitTest
{
    public class DeconflictionSingleRouteUnitTests
    {
        private static int flightId = 0;
        private static readonly Route route = new Route { RouteId = "TestRoute-0" };

        [SetUp]
        public void Setup()
        {

        }


        [Test]
        public void TrivialTest()
        {
            Assert.IsTrue(true);
        }


        [Test]
        public void TestTwoFlightsWithOverlappingStartTime()
        {
            
        }

        private Flight CreateFlight(DateTime flightStartTime)
        {
            return new Flight { FlightId = flightId++, TimeOfFlight = flightStartTime,
                                Route = route, RouteId = route.RouteId };
        }

        private Flight CreateFlight(DateTime flightStartTime, float flightSpeed)
        {
            return new Flight { FlightId = flightId++, FlightSpeed = flightSpeed,
                                TimeOfFlight = flightStartTime, Route = route,
                                RouteId = route.RouteId };
        }
    }
}
