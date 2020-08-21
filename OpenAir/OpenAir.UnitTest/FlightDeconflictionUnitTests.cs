using NUnit.Framework;
using OpenAir.Data;
using OpenAir.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenAir.UnitTest
{
    public class FlightDeconflictionUnitTests
    {
        private List<Flight> flights;

        [SetUp]
        public void SetUp()
        {
            flights = new List<Flight>();
            var flight1 = new Flight
            {
                TimeOfFlight = new DateTime(2012, 9, 10, 7, 30, 0)
            };            
            flights.Add(flight1);     
        }

        [Test]
        public void TestForTwoFlightsStartingWithin15Minutes()
        {
            var flight = new Flight { TimeOfFlight = new DateTime(2012, 9, 10, 7, 35, 0) };
            
            Assert.IsFalse(FlightDeconfliction.Deconflict(flight, flights));            
        }
    }
}
